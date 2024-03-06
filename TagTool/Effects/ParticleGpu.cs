using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Effects
{
    public static class ParticleGpu
    {
        public static void ParticleCompileGpuData(Particle particle, GameCache cache, Stream stream)
        {
            // fallback
            particle.RuntimeMSprites = new List<Particle.RuntimeMSpritesBlock> { new Particle.RuntimeMSpritesBlock() };
            particle.RuntimeMFrames = new List<Particle.RuntimeMFramesBlock> { 
                new Particle.RuntimeMFramesBlock() { RuntimeMCount = new float[] { 0.0f, 0.0f, 0.0f, 0.0f } } 
            };

            if (particle.RenderMethod.ShaderProperties.Count == 0)
            {
                new TagToolWarning("Could not compile particle gpu data: invalid render method");
                return;
            }

            var rmPostprocess = particle.RenderMethod.ShaderProperties[0];

            var texture = rmPostprocess.TryGetTextureConstantFromQueryableProperty(1); // 1 should always point to base_map

            if (texture != null && texture.Bitmap != null)
            {
                Bitmap bitmap = cache.Deserialize<Bitmap>(stream, texture.Bitmap);

                bool hasSprite = false;
                if (bitmap.Sequences.Count > 0 && 
                    (particle.FrameIndex.IsConstant() || 
                    particle.AnimationRate.RuntimeMFlags.HasFlag(Tags.ParticlePropertyScalar.EditablePropertiesFlags.Bit3)))
                {
                    hasSprite = bitmap.Sequences[particle.FirstSequenceIndex % bitmap.Sequences.Count].Sprites.Count > 0;
                }

                particle.AnimationFlags = hasSprite ? 
                    particle.AnimationFlags & ~Particle.AnimationFlagsValue.DisableFrameBlending : 
                    particle.AnimationFlags | Particle.AnimationFlagsValue.DisableFrameBlending;

                // TODO: frame index evaluate
                float frameIndexInitial = 0.0f;

                RealPoint2d spriteRegPoint = new RealPoint2d(0.5f, 0.5f);
                RealVector4d spriteRect = new RealVector4d(0.0f, 1.0f, 0.0f, 1.0f);

                float imageAspect = 1.0f;
                if (bitmap.Images.Count > 0)
                {
                    var image = bitmap.Images[0];
                    if (image.Width != image.Height)
                        imageAspect = image.Width / image.Height;
                }

                if (bitmap.Sequences.Count > 0)
                {
                    var sequence = bitmap.Sequences[particle.FirstSequenceIndex % bitmap.Sequences.Count];

                    int maxSpriteIndex = sequence.Sprites.Count;
                    if (particle.AnimationFlags.HasFlag(Particle.AnimationFlagsValue.FrameAnimationOneShot))
                        maxSpriteIndex -= 1;

                    if (maxSpriteIndex > 0)
                    {
                        float fMaxSpriteIndex = (float)Math.Floor((float)maxSpriteIndex * frameIndexInitial);

                        float sign = fMaxSpriteIndex < 0.0f ? -1.0f : 1.0f;

                        var sprite = sequence.Sprites[(int)((sign * 0.5f) + fMaxSpriteIndex) % sequence.Sprites.Count];

                        spriteRegPoint = sprite.RegistrationPoint;

                        spriteRect.I = sprite.Left;
                        spriteRect.J = sprite.Right;
                        spriteRect.K = sprite.Top;
                        spriteRect.W = sprite.Bottom;
                    }
                }

                float spriteWidth = spriteRect.J - spriteRect.I;
                float spriteHeight = spriteRect.W - spriteRect.K;
                float spritePixelWidth = 1.0f / spriteWidth;
                float spritePixelHeight = 1.0f / spriteHeight;
                spriteRegPoint += particle.CenterOffset; // centered reg

                float hAspect = 1.0f, vAspect = 1.0f;
                if (spriteWidth <= spriteHeight)
                    hAspect = spriteWidth * spritePixelHeight;
                else
                    vAspect = spriteHeight * spritePixelWidth;
                hAspect *= imageAspect;

                float nPointX = (spriteRegPoint.X * spritePixelWidth) * 2.0f - 1.0f;
                float nPointY = (spriteRegPoint.Y * spritePixelHeight) * 2.0f - 1.0f;

                float spriteGpuX = (-1.0f - nPointX) * hAspect;
                float spriteGpuY = (-1.0f - nPointY) * vAspect;
                float spriteGpuZ = ((1.0f - nPointX) * hAspect) - spriteGpuX;
                float spriteGpuW = ((1.0f - nPointY) * vAspect) - spriteGpuY;

                Particle.RuntimeMSpritesBlock spriteGpu = new Particle.RuntimeMSpritesBlock
                {
                    RuntimeGpuSpriteArray = new float[] { spriteGpuX, spriteGpuY, spriteGpuZ, spriteGpuW }
                };
                particle.RuntimeMSprites[0] = spriteGpu;

                Bitmap.Sequence firstSequence = null;
                if (bitmap.ManualSequences.Count > 0)
                {
                    firstSequence = bitmap.ManualSequences[(particle.FirstSequenceIndex + 1) % bitmap.ManualSequences.Count];
                }
                else if (bitmap.Sequences.Count > 0)
                {
                    firstSequence = bitmap.Sequences[(particle.FirstSequenceIndex + 1) % bitmap.Sequences.Count];
                }

                int runtimeFrameCount = 0;
                if (firstSequence != null)
                {
                    //int spriteCount = Math.Min(firstSequence.Sprites.Count, 15);

                    foreach (var sprite in firstSequence.Sprites)
                    {
                        Particle.RuntimeMFramesBlock newFrame = new Particle.RuntimeMFramesBlock 
                        {
                            RuntimeMCount = new float[] 
                            {
                                sprite.Left,
                                sprite.Right,
                                sprite.Right - sprite.Left,
                                sprite.Bottom - sprite.Top
                            }
                        };

                        runtimeFrameCount++;
                        particle.RuntimeMFrames.Add(newFrame);

                        if (runtimeFrameCount == 15) // limit of 15 sprites
                            break;
                    }
                }

                if (runtimeFrameCount == 0)
                {
                    runtimeFrameCount = 1;
                    Particle.RuntimeMFramesBlock newFrame = new Particle.RuntimeMFramesBlock
                    {
                        RuntimeMCount = new float[] 
                        {
                            spriteRect.I,
                            spriteRect.J,
                            spriteWidth,
                            spriteHeight
                        }
                    };
                    particle.RuntimeMFrames.Add(newFrame);
                }

                particle.RuntimeMFrames[0].RuntimeMCount[0] = (float)runtimeFrameCount;
            }
        }
    }
}
