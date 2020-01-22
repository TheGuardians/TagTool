#region License
/*
MIT License
Copyright ©2003-2006 Tao Framework Team
http://www.taoframework.com
All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion License

using System;
using System.Runtime.InteropServices;
using System.Security;

#region Aliases
using ILHANDLE = System.IntPtr;
//typedef unsigned int   ILenum;
using ILenum = System.Int32;
//typedef unsigned char  ILboolean;
using ILboolean = System.Boolean;
//typedef unsigned int   ILbitfield;
using ILbitfield = System.UInt32;
//typedef char           ILbyte;
using ILbyte = System.Byte;
//typedef short          ILshort;
using ILshort = System.Int16;
//typedef int            ILint;
using ILint = System.Int32;
//typedef int            ILsizei;
using ILsizei = System.Int32;
//typedef unsigned char  ILubyte;
using ILubyte = System.Byte;
//typedef unsigned short ILushort;
using ILushort = System.UInt16;
//typedef unsigned int   ILuint;
using ILuint = System.Int32;
//typedef float          ILfloat;
using ILfloat = System.Single;
//typedef float          ILclampf;
using ILclampf = System.Single;
//typedef double         ILdouble;
using ILdouble = System.Double;
//typedef double         ILclampd;
using ILclampd = System.Double;
//typedef void           ILvoid;
//using ILvoid = void;
using ILstring = System.String;
#endregion Aliases

namespace Sentinel
{
    #region Class Documentation
    /// <summary>
    ///     DevIL (Developer's Image Library) IL binding for .NET, implementing DevIL 1.6.8 RC2.
    /// </summary>
    #endregion Class Documentation
    public static class DevIL
    {
        // --- Fields ---
        #region Private Constants
        #region CallingConvention CALLING_CONVENTION
        /// <summary>
        ///     Specifies the calling convention.
        /// </summary>
        /// <remarks>
        ///     Specifies <see cref="CallingConvention.Winapi" />.
        /// </remarks>
        private const CallingConvention CALLING_CONVENTION = CallingConvention.Winapi;
        #endregion CallingConvention CALLING_CONVENTION
        #region string DEVIL_NATIVE_LIBRARY
        /// <summary>
        /// Specifies the DevIL native library used in the bindings
        /// </summary>
        /// <remarks>
        /// The Windows dll is specified here universally - note that
        /// under Mono the non-windows native library can be mapped using
        /// the ".config" file mechanism.  Kudos to the Mono team for this
        /// simple yet elegant solution.
        /// </remarks>
        private const string DEVIL_NATIVE_LIBRARY = "DevIL.dll";
        #endregion string DEVIL_NATIVE_LIBRARY
        #endregion Private Constants

        #region Public Constants

        /// <summary>
        /// 
        /// </summary>
        public const int IL_FALSE = 0;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TRUE = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_COLOUR_INDEX = 0x1900;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_COLOR_INDEX = 0x1900;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_RGB = 0x1907;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_RGBA = 0x1908;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_BGR = 0x80E0;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_BGRA = 0x80E1;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_LUMINANCE = 0x1909;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_LUMINANCE_ALPHA = 0x190A;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_BYTE = 0x1400;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_UNSIGNED_BYTE = 0x1401;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_SHORT = 0x1402;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_UNSIGNED_SHORT = 0x1403;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_INT = 0x1404;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_UNSIGNED_INT = 0x1405;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_FLOAT = 0x1406;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DOUBLE = 0x140A;
        /// <summary>
        /// Describes the OpenIL vendor and should be used only with ilGetString
        /// </summary>
        public const int IL_VENDOR = 0x1F00;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_LOAD_EXT = 0x1F01;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_SAVE_EXT = 0x1F02;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_VERSION_1_6_8 = 1;
        /// <summary>
        /// Used to retrive a string describing the current OpenIL version.
        /// </summary>
        public const int IL_VERSION = 168;
        /// <summary>
        /// Preserves the origin state set by ilOriginFunc.
        /// </summary>
        public const int IL_ORIGIN_BIT = 0x00000001;
        /// <summary>
        /// Preserves whether OpenIL is allowed to overwrite files when saving (set by ilEnable, ilDisable).
        /// </summary>
        public const int IL_FILE_BIT = 0x00000002;
        /// <summary>
        /// d to truecolour images (set by <see cref="ilEnable"/>, <see cref="ilDisable"/>).
        /// </summary>
        public const int IL_PAL_BIT = 0x00000004;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_FORMAT_BIT = 0x00000008;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TYPE_BIT = 0x00000010;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_COMPRESS_BIT = 0x00000020;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_LOADFAIL_BIT = 0x00000040;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_FORMAT_SPECIFIC_BIT = 0x00000080;
        /// <summary>
        /// Preserves all OpenIL states and attributes.
        /// </summary>
        public const int IL_ALL_ATTRIB_BITS = 0x000FFFFF;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PAL_NONE = 0x0400;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PAL_RGB24 = 0x0401;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PAL_RGB32 = 0x0402;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PAL_RGBA32 = 0x0403;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PAL_BGR24 = 0x0404;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PAL_BGR32 = 0x0405;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PAL_BGRA32 = 0x0406;
        /// <summary>
        /// Tells OpenIL to try to determine the type of image present in FileName, File or Lump.
        /// </summary>
        public const int IL_TYPE_UNKNOWN = 0x0000;
        /// <summary>
        /// Microsoft bitmap .
        /// </summary>
        public const int IL_BMP = 0x0420;
        /// <summary>
        /// Dr. Halo .cut image.
        /// </summary>
        public const int IL_CUT = 0x0421;
        /// <summary>
        /// Doom texture.
        /// </summary>
        public const int IL_DOOM = 0x0422;
        /// <summary>
        /// Doom flat (floor).
        /// </summary>
        public const int IL_DOOM_FLAT = 0x0423;
        /// <summary>
        /// Microsoft icon (.ico).
        /// </summary>
        public const int IL_ICO = 0x0424;
        /// <summary>
        /// Jpeg.
        /// </summary>
        public const int IL_JPG = 0x0425;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_JFIF = 0x0425;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_LBM = 0x0426;
        /// <summary>
        /// Kodak PhotoCD image.
        /// </summary>
        public const int IL_PCD = 0x0427;
        /// <summary>
        ///  .pcx Image.
        /// </summary>
        public const int IL_PCX = 0x0428;
        /// <summary>
        /// Softimage Pic image.
        /// </summary>
        public const int IL_PIC = 0x0429;
        /// <summary>
        /// Portable Network Graphics (.png) image.
        /// </summary>
        public const int IL_PNG = 0x042A;
        /// <summary>
        /// Portable AnyMap (.pbm, .pgm or .ppm).
        /// </summary>
        public const int IL_PNM = 0x042B;
        /// <summary>
        /// SGI (.bw, .rgb, .rgba or .sgi).
        /// </summary>
        public const int IL_SGI = 0x042C;
        /// <summary>
        /// TrueVision Targa.
        /// </summary>
        public const int IL_TGA = 0x042D;
        /// <summary>
        /// TIFF (.tif or .tiff) image.
        /// </summary>
        public const int IL_TIF = 0x042E;
        /// <summary>
        /// C Header.
        /// </summary>
        public const int IL_CHEAD = 0x042F;
        /// <summary>
        /// Raw data with a 13-byte header.
        /// </summary>
        public const int IL_RAW = 0x0430;
        /// <summary>
        /// Half-Life model file (.mdl).
        /// </summary>
        public const int IL_MDL = 0x0431;
        /// <summary>
        /// Quake .wal texture.
        /// </summary>
        public const int IL_WAL = 0x0432;
        /// <summary>
        /// Homeworld image.
        /// </summary>
        public const int IL_LIF = 0x0434;
        /// <summary>
        /// Load a Multiple Network Graphics (.mng).
        /// </summary>
        public const int IL_MNG = 0x0435;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_JNG = 0x0435;
        /// <summary>
        /// Graphics Interchange Format file.
        /// </summary>
        public const int IL_GIF = 0x0436;
        /// <summary>
        /// DirectDraw Surface image.
        /// </summary>
        public const int IL_DDS = 0x0437;
        /// <summary>
        /// .dcx image.
        /// </summary>
        public const int IL_DCX = 0x0438;
        /// <summary>
        /// PhotoShop (.psd) file.
        /// </summary>
        public const int IL_PSD = 0x0439;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_EXIF = 0x043A;
        /// <summary>
        /// Paint Shop Pro file.
        /// </summary>
        public const int IL_PSP = 0x043B;
        /// <summary>
        /// Alias | Wavefront .pix file.
        /// </summary>
        public const int IL_PIX = 0x043C;
        /// <summary>
        /// Pxrar (.pxr) file.
        /// </summary>
        public const int IL_PXR = 0x043D;
        /// <summary>
        /// .xpm file.
        /// </summary>
        public const int IL_XPM = 0x043E;
        /// <summary>
        /// RADIANCE High Dynamic Range Image.
        /// </summary>
        public const int IL_HDR = 0x043F;
        /// <summary>
        /// Load the file into the current image's palette as a Paint Shop Pro (Jasc) palette.
        /// </summary>
        public const int IL_JASC_PAL = 0x0475;
        /// <summary>
        /// No detectable error has occured.
        /// </summary>
        public const int IL_NO_ERROR = 0x0000;
        /// <summary>
        /// An invalid value have been used, which was not part of the set of values that can be used. In the function documentation there should be a more specific descriptionanation.
        /// </summary>
        public const int IL_INVALID_ENUM = 0x0501;
        /// <summary>
        /// Could not allocate enough memory for the image data.
        /// </summary>
        public const int IL_OUT_OF_MEMORY = 0x0502;
        /// <summary>
        /// The format a function tried to use was not able to be used by that function.
        /// </summary>
        public const int IL_FORMAT_NOT_SUPPORTED = 0x0503;
        /// <summary>
        /// A serious error has occurred.
        /// </summary>
        public const int IL_INTERNAL_ERROR = 0x0504;
        /// <summary>
        /// An invalid value was passed to a function or was in a file.
        /// </summary>
        public const int IL_INVALID_VALUE = 0x0505;
        /// <summary>
        /// The operation attempted is not allowable in the current state. The function returns with no ill side effects. Generally there is currently no image bound or it has been deleted via ilDeleteImages. You should use ilGenImages and ilBindImage before calling the function.
        /// </summary>
        public const int IL_ILLEGAL_OPERATION = 0x0506;
        /// <summary>
        /// An illegal value was found in a file trying to be loaded.
        /// </summary>
        public const int IL_ILLEGAL_FILE_VALUE = 0x0507;
        /// <summary>
        ///  	s header was incorrect.
        /// </summary>
        public const int IL_INVALID_FILE_HEADER = 0x0508;
        /// <summary>
        /// An invalid value have been used, which was not part of the set of values that can be used. In the function documentation there should be a more specific descriptionanation.
        /// </summary>
        public const int IL_INVALID_PARAM = 0x0509;
        /// <summary>
        /// Could not open the file specified. The file may already be open by another app or may not exist.
        /// </summary>
        public const int IL_COULD_NOT_OPEN_FILE = 0x050A;
        /// <summary>
        /// The extension of the specified filename was not correct for the type of image-loading function.
        /// </summary>
        public const int IL_INVALID_EXTENSION = 0x050B;
        /// <summary>
        /// The filename specified already belongs to another file. To overwrite files by default read more at ilEnable function.
        /// </summary>
        public const int IL_FILE_ALREADY_EXISTS = 0x050C;
        /// <summary>
        /// Tried to convert an image from its format to the same format.
        /// </summary>
        public const int IL_OUT_FORMAT_SAME = 0x050D;
        /// <summary>
        /// One of the internal stacks was already filled, and the user tried to add on to the full stack.
        /// </summary>
        public const int IL_STACK_OVERFLOW = 0x050E;
        /// <summary>
        /// One of the internal stacks was empty, and the user tried to empty the already empty stack.
        /// </summary>
        public const int IL_STACK_UNDERFLOW = 0x050F;
        /// <summary>
        /// During a conversion destination format and/or dest type was an invalid identifier. In the function documentation there should be a more specific descriptionanation.
        /// </summary>
        public const int IL_INVALID_CONVERSION = 0x0510;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_BAD_DIMENSIONS = 0x0511;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_FILE_READ_ERROR = 0x0512;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_FILE_WRITE_ERROR = 0x0512;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_LIB_GIF_ERROR = 0x05E1;
        /// <summary>
        /// An error occurred in the libjpeg library.
        /// </summary>
        public const int IL_LIB_JPEG_ERROR = 0x05E2;
        /// <summary>
        /// An error occurred in the libpng library.
        /// </summary>
        public const int IL_LIB_PNG_ERROR = 0x05E3;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_LIB_TIFF_ERROR = 0x05E4;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_LIB_MNG_ERROR = 0x05E5;
        /// <summary>
        /// No function sets this yet, but it is possible (not probable) it may be used in the future.
        /// </summary>
        public const int IL_UNKNOWN_ERROR = 0x05FF;
        /// <summary>
        /// nabled, the origin is specified at an absolute position, and all images loaded or saved adhere to this set origin. For more information, check out ilOriginFunc.
        /// </summary>
        public const int IL_ORIGIN_SET = 0x0600;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_ORIGIN_LOWER_LEFT = 0x0601;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_ORIGIN_UPPER_LEFT = 0x0602;
        /// <summary>
        /// Returns the current origin position.
        /// </summary>
        public const int IL_ORIGIN_MODE = 0x0603;
        /// <summary>
        /// Returns whether all images loaded are converted to a specific format.
        /// </summary>
        public const int IL_FORMAT_SET = 0x0610;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_FORMAT_MODE = 0x0611;
        /// <summary>
        /// Returns whether all images loaded are converted to a specific type.
        /// </summary>
        public const int IL_TYPE_SET = 0x0612;
        /// <summary>
        /// Returns the type images are converted to upon loading.
        /// </summary>
        public const int IL_TYPE_MODE = 0x0613;

        /// <summary>
        /// 
        /// </summary>
        public const int IL_FILE_OVERWRITE = 0x0620;
        /// <summary>
        /// Returns whether file overwriting when saving is enabled.
        /// </summary>
        public const int IL_FILE_MODE = 0x0621;
        /// <summary>
        /// d images to their base types, e.g. converting to a bgra image.
        /// </summary>
        public const int IL_CONV_PAL = 0x0630;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DEFAULT_ON_FAIL = 0x0632;
        /// <summary>
        /// Returns whether OpenIL uses a key colour (not used yet).
        /// </summary>
        public const int IL_USE_KEY_COLOUR = 0x0635;
        /// <summary>
        /// Returns whether OpenIL uses a key colour (not used yet).
        /// </summary>
        public const int IL_USE_KEY_COLOR = 0x0635;
        /// <summary>
        /// /
        /// </summary>
        public const int IL_SAVE_INTERLACED = 0x0639;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_INTERLACE_MODE = 0x063A;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_QUANTIZATION_MODE = 0x0640;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_WU_QUANT = 0x0641;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_NEU_QUANT = 0x0642;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_NEU_QUANT_SAMPLE = 0x0643;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_MAX_QUANT_INDEXS = 0x0644;
        /// <summary>
        /// Makes the target use a faster but more memory-intensive algorithm.
        /// </summary>
        public const int IL_FASTEST = 0x0660;
        /// <summary>
        /// Makes the target use less memory but a potentially slower algorithm.
        /// </summary>
        public const int IL_LESS_MEM = 0x0661;
        /// <summary>
        /// The client does not have a preference.
        /// </summary>
        public const int IL_DONT_CARE = 0x0662;
        /// <summary>
        /// Controls the memory used vs. speed tradeoff.
        /// </summary>
        public const int IL_MEM_SPEED_HINT = 0x0665;
        /// <summary>
        /// Specifies that OpenIL should use compression when saving, if possible.
        /// </summary>
        public const int IL_USE_COMPRESSION = 0x0666;
        /// <summary>
        /// Specifies that OpenIL should never use compression when saving.
        /// </summary>
        public const int IL_NO_COMPRESSION = 0x0667;
        /// <summary>
        /// Controls whether compression is used when saving images.
        /// </summary>
        public const int IL_COMPRESSION_HINT = 0x0668;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_SUB_NEXT = 0x0680;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_SUB_MIPMAP = 0x0681;

        /// <summary>
        /// /
        /// </summary>
        public const int IL_SUB_LAYER = 0x0682;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_COMPRESS_MODE = 0x0700;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_COMPRESS_NONE = 0x0701;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_COMPRESS_RLE = 0x0702;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_COMPRESS_LZO = 0x0703;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_COMPRESS_ZLIB = 0x0704;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TGA_CREATE_STAMP = 0x0710;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_JPG_QUALITY = 0x0711;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PNG_INTERLACE = 0x0712;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TGA_RLE = 0x0713;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_BMP_RLE = 0x0714;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_SGI_RLE = 0x0715;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TGA_ID_STRING = 0x0717;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TGA_AUTHNAME_STRING = 0x0718;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TGA_AUTHCOMMENT_STRING = 0x0719;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PNG_AUTHNAME_STRING = 0x071A;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PNG_TITLE_STRING = 0x071B;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PNG_DESCRIPTION_STRING = 0x071C;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TIF_DESCRIPTION_STRING = 0x071D;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TIF_HOSTCOMPUTER_STRING = 0x071E;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TIF_DOCUMENTNAME_STRING = 0x071F;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_TIF_AUTHNAME_STRING = 0x0720;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_JPG_SAVE_FORMAT = 0x0721;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_CHEAD_HEADER_STRING = 0x0722;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PCD_PICNUM = 0x0723;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PNG_ALPHA_INDEX = 0x0724;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DXTC_FORMAT = 0x0705;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DXT1 = 0x0706;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DXT2 = 0x0707;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DXT3 = 0x0708;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DXT4 = 0x0709;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DXT5 = 0x070A;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DXT_NO_COMP = 0x070B;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_KEEP_DXTC_DATA = 0x070C;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_DXTC_DATA_FORMAT = 0x070D;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_3DC = 0x070E;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_RXGB = 0x070F;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_ATI1N = 0x0710;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_CUBEMAP_POSITIVEX = 0x00000400;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_CUBEMAP_NEGATIVEX = 0x00000800;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_CUBEMAP_POSITIVEY = 0x00001000;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_CUBEMAP_NEGATIVEY = 0x00002000;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_CUBEMAP_POSITIVEZ = 0x00004000;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_CUBEMAP_NEGATIVEZ = 0x00008000;
        /// <summary>
        /// Returns the version number of the shared library. This can be checked against the IL_VERSION #define.
        /// </summary>
        public const int IL_VERSION_NUM = 0x0DE2;
        /// <summary>
        /// s width.
        /// </summary>
        public const int IL_IMAGE_WIDTH = 0x0DE4;
        /// <summary>
        /// s height.
        /// </summary>
        public const int IL_IMAGE_HEIGHT = 0x0DE5;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_DEPTH = 0x0DE6;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_SIZE_OF_DATA = 0x0DE7;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_BPP = 0x0DE8;
        /// <summary>
        /// s data.
        /// </summary>
        public const int IL_IMAGE_BYTES_PER_PIXEL = 0x0DE8;
        /// <summary>
        /// s data.
        /// </summary>
        public const int IL_IMAGE_BITS_PER_PIXEL = 0x0DE9;
        /// <summary>
        /// Returns the current image format.
        /// </summary>
        public const int IL_IMAGE_FORMAT = 0x0DEA;
        /// <summary>
        /// Returns the current images type.
        /// </summary>
        public const int IL_IMAGE_TYPE = 0x0DEB;
        /// <summary>
        /// Returns the palette type of the current image.
        /// </summary>
        public const int IL_PALETTE_TYPE = 0x0DEC;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PALETTE_SIZE = 0x0DED;
        /// <summary>
        /// Returns the bytes per pixel of the current images palette.
        /// </summary>
        public const int IL_PALETTE_BPP = 0x0DEE;
        /// <summary>
        /// Returns the number of colours of the current images palette.
        /// </summary>
        public const int IL_PALETTE_NUM_COLS = 0x0DEF;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_PALETTE_BASE_TYPE = 0x0DF0;
        /// <summary>
        /// Returns the number of images in the current image animation chain.
        /// </summary>
        public const int IL_NUM_IMAGES = 0x0DF1;
        /// <summary>
        /// Returns the number of mipmaps of the current image.
        /// </summary>
        public const int IL_NUM_MIPMAPS = 0x0DF2;
        /// <summary>
        /// /
        /// </summary>
        public const int IL_NUM_LAYERS = 0x0DF3;
        /// <summary>
        /// Returns the current image number.
        /// </summary>
        public const int IL_ACTIVE_IMAGE = 0x0DF4;
        /// <summary>
        /// Returns the current mipmap number.
        /// </summary>
        public const int IL_ACTIVE_MIPMAP = 0x0DF5;
        /// <summary>
        /// Returns the current layer number.
        /// </summary>
        public const int IL_ACTIVE_LAYER = 0x0DF6;
        /// <summary>
        /// Returns the current bound image name.
        /// </summary>
        public const int IL_CUR_IMAGE = 0x0DF7;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_DURATION = 0x0DF8;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_PLANESIZE = 0x0DF9;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_BPC = 0x0DFA;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_OFFX = 0x0DFB;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_OFFY = 0x0DFC;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_CUBEFLAGS = 0x0DFD;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_ORIGIN = 0x0DFE;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_IMAGE_CHANNELS = 0x0DFF;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_SEEK_SET = 0;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_SEEK_CUR = 1;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_SEEK_END = 2;
        /// <summary>
        /// 
        /// </summary>
        public const int IL_EOF = -1;

        #endregion Public Constants

        #region Delegates

        // Callback functions for file reading
        //typedef ILvoid    (ILAPIENTRY *fCloseRProc)(ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void fCloseRProc(ILHANDLE handle);
        //typedef ILboolean (ILAPIENTRY *fEofProc)   (ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILboolean fEofProc(ILHANDLE handle);
        //typedef ILint     (ILAPIENTRY *fGetcProc)  (ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILint fGetcProc(ILHANDLE handle);
        //typedef ILHANDLE  (ILAPIENTRY *fOpenRProc) (const ILstring);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILHANDLE fOpenRProc(ILstring str);
        //typedef ILint     (ILAPIENTRY *fReadProc)  (void*, ILuint, ILuint, ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILint fReadProc(IntPtr ptr, ILuint a, ILuint b, ILHANDLE handle);
        //typedef ILint     (ILAPIENTRY *fSeekRProc) (ILHANDLE, ILint, ILint);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILint fSeekRProc(ILHANDLE handle, ILint a, ILint b);
        //typedef ILint     (ILAPIENTRY *fTellRProc) (ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILint fTellRProc(ILHANDLE handle);

        // Callback functions for file writing
        //typedef ILvoid   (ILAPIENTRY *fCloseWProc)(ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void fCloseWProc(ILHANDLE handle);
        //typedef ILHANDLE (ILAPIENTRY *fOpenWProc) (const ILstring);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILHANDLE fOpenWProc(ILstring str);
        //typedef ILint    (ILAPIENTRY *fPutcProc)  (ILubyte, ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="byt"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILint fPutcProc(ILubyte byt, ILHANDLE handle);
        //typedef ILint    (ILAPIENTRY *fSeekWProc) (ILHANDLE, ILint, ILint);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILint fSeekWProc(ILHANDLE handle, ILint a, ILint b);
        //typedef ILint    (ILAPIENTRY *fTellWProc) (ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILint fTellWProc(ILHANDLE handle);
        //typedef ILint    (ILAPIENTRY *fWriteProc) (const void*, ILuint, ILuint, ILHANDLE);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILint fWriteProc(IntPtr ptr, ILuint a, ILuint b, ILHANDLE handle);

        // Callback functions for allocation and deallocation
        //typedef ILvoid* (ILAPIENTRY *mAlloc)(ILuint);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void mAlloc(ILuint a);
        //typedef ILvoid  (ILAPIENTRY *mFree) (ILvoid*);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void mFree(IntPtr ptr);

        // Registered format procedures
        //typedef ILenum (ILAPIENTRY *IL_LOADPROC)(const ILstring);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILenum IL_LOADPROC(ILstring str);
        //typedef ILenum (ILAPIENTRY *IL_SAVEPROC)(const ILstring);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate ILenum IL_SAVEPROC(ILstring str);

        #endregion Delegates

        #region Externs

        // ImageLib Functions
        /// <summary>
        /// ilActiveImage sets the current image to be an image in an animation chain
        /// </summary>
        /// <param name="Number">Animation numer to select as current.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilActiveImage(ILuint Number);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilActiveImage(ILuint Number);

        /// <summary>
        /// ilActiveLayer is not yet used.
        /// </summary>
        /// <param name="Number">Layer number to select as current.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilActiveLayer(ILuint Number);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilActiveLayer(ILuint Number);

        /// <summary>
        /// ilActiveMipmap sets a mipmap of the image as the current mipmap. Currently, the only way to generate mipmaps is by calling iluBuildMipmaps. If neither function has been called for the current image, no mipmaps exist for it. If Number is 0, then the current base image is set.
        /// </summary>
        /// <param name="Number">Mipmap level to select as current.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilActiveMipmap(ILuint Number);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilActiveMipmap(ILuint Number);

        // ILAPI ILboolean ILAPIENTRY ilApplyPal(const ILstring FileName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilApplyPal(ILstring FileName);

        /// <summary>
        /// iluApplyProfile applies a colour profile (files with extension .icm) to the currently bound image. InProfile describes the current image's colour space, and OutProfile describes the colour space to convert the currently bound image to. If InProfile is NULL, DevIL attempts to use the colour profile present in the image, if one is present, else it returns IL_FALSE.
        /// </summary>
        /// <param name="InProfile">Profile file describing the colour space the image is in.</param>
        /// <param name="OutProfile">Profile file describing the colour space to convert the image to.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilApplyProfile(ILstring InProfile, ILstring OutProfile);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilApplyProfile(ILstring InProfile, ILstring OutProfile);

        /// <summary>
        /// s, with zero being reserved as the default image. The default image is generated by ilCreateDefaultTex. The only reason the default image would be NULL is if OpenIL could not create the default image, due to memory constraints of the system, so always heed the IL_OUT_OF_MEMORY error. Any dimension image may be bound with ilBindImage. When ilBindImage is called, the bound image remains bound until ilBindImage is called again with a different value in Image.
        /// </summary>
        /// <param name="Image">The name of an image.</param>
        // ILAPI ILvoid    ILAPIENTRY ilBindImage(ILuint Image);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilBindImage(ILuint Image);

        // ILAPI ILboolean ILAPIENTRY ilBlit(ILuint Source, ILint DestX, ILint DestY, ILint DestZ, ILuint SrcX, ILuint SrcY, ILuint SrcZ, ILuint Width, ILuint Height, ILuint Depth);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="DestX"></param>
        /// <param name="DestY"></param>
        /// <param name="DestZ"></param>
        /// <param name="SrcX"></param>
        /// <param name="SrcY"></param>
        /// <param name="SrcZ"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Depth"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilBlit(ILuint Source, ILint DestX, ILint DestY, ILint DestZ, ILuint SrcX, ILuint SrcY, ILuint SrcZ, ILuint Width, ILuint Height, ILuint Depth);

        /// <summary>
        /// ilClearColour sets the current clearing colour to be used by future calls to ilClearImage. iluRotate and iluEnlargeCanvas both use these values to clear blank space in images, too.
        /// </summary>
        /// <param name="Red">Amount of red to clear to.</param>
        /// <param name="Green">Amount of green to clear to.</param>
        /// <param name="Blue">Amount of blue to clear to.</param>
        /// <param name="Alpha">Amount of alpha to clear to.</param>
        // ILAPI ILvoid    ILAPIENTRY ilClearColour(ILclampf Red, ILclampf Green, ILclampf Blue, ILclampf Alpha);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilClearColour(ILclampf Red, ILclampf Green, ILclampf Blue, ILclampf Alpha);

        /// <summary>
        /// ilClearImage simply clears the image to the colours specified in ilClearColour. If the current image is of format IL_COLOR_INDEX, the image is cleared to all zeros, and the palette is changed to one entry of all zeros. If the current image is of format IL_LUMINANCE, the image is cleared to all zeros.
        /// </summary>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilClearImage(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilClearImage();

        /// <summary>
        /// ilCloneCurImage creates a copy of the current image and returns the id of the new image. If a subimage of the current image is currently selected via ilActiveImage, ilActiveLayer or ilActiveMipmap, the subimage is copied, not the base image.
        /// </summary>
        /// <returns></returns>
        // ILAPI ILuint    ILAPIENTRY ilCloneCurImage(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILuint ilCloneCurImage();

        // ILAPI ILboolean ILAPIENTRY ilCompressFunc(ILenum Mode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilCompressFunc(ILenum Mode);

        /// <summary>
        /// ilConvertImage converts the current bound image from its format/type to DestFormat and DestType. Almost all conversions are allowable.
        /// </summary>
        /// <param name="DestFormat">The format the current image should be converted to.</param>
        /// <param name="DestType">The type the current image should be converted to.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilConvertImage(ILenum DestFormat, ILenum DestType);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilConvertImage(ILenum DestFormat, ILenum DestType);

        /// <summary>
        /// ilIsImage returns whether the image name in Image is a valid image in use. If the image name in Image is in use, ilIsImage returns IL_TRUE. If Image is 0, ilIsImage returns IL_FALSE, because the default image is a special image and is never returned by ilGenImages. If the image name has been deleted by ilDeleteImages or never generated byilGenImages, IL_FALSE is returned.
        /// </summary>
        /// <param name="DestFormat">The format the current image palette should be converted to. Accepted Values are: <see cref="IL_PAL_RGB24"/>, <see cref="IL_PAL_RGB32"/>, <see cref="IL_PAL_RGBA32"/>, <see cref="IL_PAL_BGR24"/>, <see cref="IL_PAL_BGR32"/>, <see cref="IL_PAL_BGRA32"/>.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilConvertPal(ILenum DestFormat);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilConvertPal(ILenum DestFormat);

        /// <summary>
        /// ilCopyImage copies the attributes and data from the image named in Src. The same image bound before calling ilCopyImage remains bound afterward.
        /// </summary>
        /// <param name="Src">Name of an image to copy to the current image.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilCopyImage(ILuint Src);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilCopyImage(ILuint Src);

        // ILAPI ILuint    ILAPIENTRY ilCopyPixels(ILuint XOff, ILuint YOff, ILuint ZOff, ILuint Width, ILuint Height, ILuint Depth, ILenum Format, ILenum Type, ILvoid *Data);
        /// <summary>
        /// s width, height or depth number of pixels will be copied to Data.
        /// </summary>
        /// <param name="XOff">Where to begin copying pixels from in the x direction.</param>
        /// <param name="YOff">Where to begin copying pixels from in the y direction.</param>
        /// <param name="ZOff">Where to begin copying pixels from in the z direction.</param>
        /// <param name="Width">How many pixels to copy in the x direction.</param>
        /// <param name="Height">How many pixels to copy in the y direction.</param>
        /// <param name="Depth">How many pixels to copy in the z direction.</param>
        /// <param name="Format">The desired format the output should be.</param>
        /// <param name="Type">The desired type the output should be.</param>
        /// <param name="Data">User-defined buffer to copy the image data to.</param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILuint ilCopyPixels(ILuint XOff, ILuint YOff, ILuint ZOff, ILuint Width, ILuint Height, ILuint Depth, ILenum Format, ILenum Type, IntPtr Data);

        // ILAPI ILuint    ILAPIENTRY ilCreateSubImage(ILenum Type, ILuint Num);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILuint ilCreateSubImage(ILenum Type, ILuint Num);

        /// <summary>
        /// ilDefaultImage creates an ugly 64x64 image of 8x8 black and yellow squares to form a checkerboard pattern. In future versions of OpenIL, there may be an option that will load this image if an image-loading function failed (unless memory could not be allocated). This way, the user can easily tell if an image was not loaded. Plus, the calling program can continue normally, even though it will have an ugly image.
        /// </summary>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilDefaultImage(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilDefaultImage();

        // ILAPI ILvoid    ILAPIENTRY ilDeleteImage(const ILuint Num);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Num"></param>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilDeleteImage(ILuint Num);

        /// <summary>
        /// ilDeleteImages deletes Num image names specified in Image. After a texture is deleted, its characteristics and dimensions are undefined, and the name may be reused byilGenImages. ilDeleteImages ignores zeros and out-of-bounds image names. If the current image is deleted, the binding reverts to the default image (image name of 0).
        /// </summary>
        /// <param name="Num">Number of image names to delete.</param>
        /// <param name="Image">Pointer to image names to delete.</param>
        // ILAPI ILvoid    ILAPIENTRY ilDeleteImages(ILsizei Num, const ILuint *Images);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilDeleteImages(ILsizei Num, ref ILuint Image);

        /// <summary>
        /// ilDeleteImages deletes Num image names specified in Images. After a texture is deleted, its characteristics and dimensions are undefined, and the name may be reused byilGenImages. ilDeleteImages ignores zeros and out-of-bounds image names. If the current image is deleted, the binding reverts to the default image (image name of 0).
        /// </summary>
        /// <param name="Num">Number of image names to delete.</param>
        /// <param name="Images">Pointer to image names to delete.</param>
        // ILAPI ILvoid    ILAPIENTRY ilDeleteImages(ILsizei Num, const ILuint *Images);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilDeleteImages(ILsizei Num, ILuint[] Images);

        /// <summary>
        /// s OpenGL counterpart glDisable.
        /// </summary>
        /// <param name="Mode">Mode to disable.</param>
        /// <returns></returns>
        /// <remarks>
        /// <para><see cref="IL_CONV_PAL"/> - When enabled, OpenIL automatically converts palette'd images to their base types, e.g. converting to a bgra image.</para>
        /// <para><see cref="IL_FILE_OVERWRITE"/> - If enabled while saving, OpenIL will overwrite existing files, else <see cref="IL_FILE_ALREADY_EXISTS"/> is set, and the image is not saved.</para>
        /// <para><see cref="IL_ORIGIN_SET"/> - When enabled, the origin is specified at an absolute position, and all images loaded or saved adhere to this set origin. For more information, check out <see cref="ilOriginFunc"/>. </para>
        /// </remarks>
        // ILAPI ILboolean ILAPIENTRY ilDisable(ILenum Mode);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilDisable(ILenum Mode);

        /// <summary>
        /// s OpenGL counterpart glEnable.
        /// </summary>
        /// <param name="Mode">Mode to enable.</param>
        /// <returns></returns>
        /// <remarks>
        /// <para><see cref="IL_CONV_PAL"/> - When enabled, OpenIL automatically converts palette'd images to their base types, e.g. converting to a bgra image.</para>
        /// <para><see cref="IL_FILE_OVERWRITE"/> - If enabled while saving, OpenIL will overwrite existing files, else <see cref="IL_FILE_ALREADY_EXISTS"/> is set, and the image is not saved.</para>
        /// <para><see cref="IL_ORIGIN_SET"/> - When enabled, the origin is specified at an absolute position, and all images loaded or saved adhere to this set origin. For more information, check out <see cref="ilOriginFunc"/>. </para>
        /// </remarks>
        // ILAPI ILboolean ILAPIENTRY ilEnable(ILenum Mode);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilEnable(ILenum Mode);

        // ILAPI ILboolean ILAPIENTRY ilFormatFunc(ILenum Mode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilFormatFunc(ILenum Mode);

        /// <summary>
        /// ilGenImages stores Num image names in Images. The names stored are not necessarily contiguous, and names can have been deleted via ilDeleteImages beforehand. The image names stored in Images can be used with ilBindImage after calling ilGenImages. After calling ilGenImages, all image dimensions and features are undefined.
        /// </summary>
        /// <param name="Num">Number of image names to generate.</param>
        /// <param name="Images">Pointer in which the generated image names are stored.</param>
        // ILAPI ILvoid    ILAPIENTRY ilGenImages(ILsizei Num, ILuint *Images);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilGenImages(ILsizei Num, out ILuint Images);

        /// <summary>
        /// ilGenImages stores Num image names in Images. The names stored are not necessarily contiguous, and names can have been deleted via ilDeleteImages beforehand. The image names stored in Images can be used with ilBindImage after calling ilGenImages. After calling ilGenImages, all image dimensions and features are undefined.
        /// </summary>
        /// <param name="Num">Number of image names to generate.</param>
        /// <param name="Images">Pointer in which the generated image names are stored.</param>
        // ILAPI ILvoid    ILAPIENTRY ilGenImages(ILsizei Num, ILuint *Images);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilGenImages(ILsizei Num, [Out] ILuint[] Images);

        // ILAPI ILint		ILAPIENTRY ilGenImage();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILint ilGenImage();

        // ILAPI ILubyte*  ILAPIENTRY ilGetAlpha(ILenum Type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr ilGetAlpha(ILenum Type);

        /// <summary>
        /// ilGetBoolean returns the value of a selected mode.
        /// </summary>
        /// <param name="Mode">The mode value to be returned.</param>
        /// <returns></returns>
        /// <remarks>
        /// <para><see cref="IL_ACTIVE_IMAGE"/> - Returns the current image number.</para>
        /// <para><see cref="IL_ACTIVE_LAYER"/> - Returns the current layer number.</para>
        /// <para><see cref="IL_ACTIVE_MIPMAP"/> - Returns the current mipmap number..</para>
        /// <para><see cref="IL_CONV_PAL"/> - Returns whether palette'd images are converted to their base palettes types on load - e.g. converted to a bgra image.</para>
        /// <para><see cref="IL_CUR_IMAGE"/> - Returns the current bound image name.</para>
        /// <para><see cref="IL_FILE_MODE"/> - Returns whether file overwriting when saving is enabled.</para>
        /// <para><see cref="IL_FORMAT_MODE"/> - Returns the format images are converted to upon loading.</para>
        /// <para><see cref="IL_FORMAT_SET"/> - Returns whether all images loaded are converted to a specific format.</para>
        /// <para><see cref="IL_IMAGE_BITS_PER_PIXEL"/> - Returns the bits per pixel of the current image's data.</para>
        /// <para><see cref="IL_IMAGE_BYTES_PER_PIXEL"/> - Returns the bytes per pixel of the current image's data.</para>
        /// <para><see cref="IL_IMAGE_FORMAT"/> - Returns the current image's format.</para>
        /// <para><see cref="IL_IMAGE_HEIGHT"/> - Returns the current image's height.</para>
        /// <para><see cref="IL_IMAGE_TYPE"/> - Returns the current image's type.</para>
        /// <para><see cref="IL_IMAGE_WIDTH"/> - Returns the current image's width.</para>
        /// <para><see cref="IL_NUM_IMAGES"/> - Returns the number of images in the current image animation chain.</para>
        /// <para><see cref="IL_NUM_MIPMAPS"/> - Returns the number of mipmaps of the current image.</para>
        /// <para><see cref="IL_ORIGIN_MODE"/> - Returns the current origin position.</para>
        /// <para><see cref="IL_ORIGIN_SET"/> - Returns whether all images loaded and saved adhere to a specific origin.</para>
        /// <para><see cref="IL_PALETTE_BPP"/> - Returns the bytes per pixel of the current image's palette.</para>
        /// <para><see cref="IL_PALETTE_NUM_COLS"/> - Returns the number of colours of the current image's palette.</para>
        /// <para><see cref="IL_PALETTE_TYPE"/> - Returns the palette type of the current image.</para>
        /// <para><see cref="IL_TYPE_MODE"/> - Returns the type images are converted to upon loading.</para>
        /// <para><see cref="IL_TYPE_SET"/> - Returns whether all images loaded are converted to a specific type.</para>
        /// <para><see cref="IL_USE_KEY_COLOUR"/> - Returns whether OpenIL uses a key colour (not used yet).</para>
        /// <para><see cref="IL_VERSION_NUM"/> - Returns the version number of the shared library. This can be checked against the <see cref="IL_VERSION"/> #define.</para>
        /// </remarks>
        // ILAPI ILboolean ILAPIENTRY ilGetBoolean(ILenum Mode);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilGetBoolean(ILenum Mode);

        /// <summary>
        /// ilGetBooleanv function returns the mode value in the Param parameter.
        /// </summary>
        /// <param name="Mode">The mode value to be returned.</param>
        /// <param name="Param">Array where the values are stored</param>
        /// <remarks>
        /// <para><see cref="IL_ACTIVE_IMAGE"/> - Returns the current image number.</para>
        /// <para><see cref="IL_ACTIVE_LAYER"/> - Returns the current layer number.</para>
        /// <para><see cref="IL_ACTIVE_MIPMAP"/> - Returns the current mipmap number..</para>
        /// <para><see cref="IL_CONV_PAL"/> - Returns whether palette'd images are converted to their base palettes types on load - e.g. converted to a bgra image.</para>
        /// <para><see cref="IL_CUR_IMAGE"/> - Returns the current bound image name.</para>
        /// <para><see cref="IL_FILE_MODE"/> - Returns whether file overwriting when saving is enabled.</para>
        /// <para><see cref="IL_FORMAT_MODE"/> - Returns the format images are converted to upon loading.</para>
        /// <para><see cref="IL_FORMAT_SET"/> - Returns whether all images loaded are converted to a specific format.</para>
        /// <para><see cref="IL_IMAGE_BITS_PER_PIXEL"/> - Returns the bits per pixel of the current image's data.</para>
        /// <para><see cref="IL_IMAGE_BYTES_PER_PIXEL"/> - Returns the bytes per pixel of the current image's data.</para>
        /// <para><see cref="IL_IMAGE_FORMAT"/> - Returns the current image's format.</para>
        /// <para><see cref="IL_IMAGE_HEIGHT"/> - Returns the current image's height.</para>
        /// <para><see cref="IL_IMAGE_TYPE"/> - Returns the current image's type.</para>
        /// <para><see cref="IL_IMAGE_WIDTH"/> - Returns the current image's width.</para>
        /// <para><see cref="IL_NUM_IMAGES"/> - Returns the number of images in the current image animation chain.</para>
        /// <para><see cref="IL_NUM_MIPMAPS"/> - Returns the number of mipmaps of the current image.</para>
        /// <para><see cref="IL_ORIGIN_MODE"/> - Returns the current origin position.</para>
        /// <para><see cref="IL_ORIGIN_SET"/> - Returns whether all images loaded and saved adhere to a specific origin.</para>
        /// <para><see cref="IL_PALETTE_BPP"/> - Returns the bytes per pixel of the current image's palette.</para>
        /// <para><see cref="IL_PALETTE_NUM_COLS"/> - Returns the number of colours of the current image's palette.</para>
        /// <para><see cref="IL_PALETTE_TYPE"/> - Returns the palette type of the current image.</para>
        /// <para><see cref="IL_TYPE_MODE"/> - Returns the type images are converted to upon loading.</para>
        /// <para><see cref="IL_TYPE_SET"/> - Returns whether all images loaded are converted to a specific type.</para>
        /// <para><see cref="IL_USE_KEY_COLOUR"/> - Returns whether OpenIL uses a key colour (not used yet).</para>
        /// <para><see cref="IL_VERSION_NUM"/> - Returns the version number of the shared library. This can be checked against the <see cref="IL_VERSION"/> #define.</para>
        /// </remarks>
        // ILAPI ILvoid    ILAPIENTRY ilGetBooleanv(ILenum Mode, ILboolean *Param);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilGetBooleanv(ILenum Mode, out ILboolean Param);

        /// <summary>
        /// s data to allow direct access and modification to the contents of the image.
        /// </summary>
        /// <returns></returns>
        // ILAPI ILubyte*  ILAPIENTRY ilGetData(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr ilGetData();

        // ILAPI ILuint    ILAPIENTRY ilGetDXTCData(ILvoid *Buffer, ILuint BufferSize, ILenum DXTCFormat);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        /// <param name="BufferSize"></param>
        /// <param name="DXTCFormat"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILuint ilGetDXTCData(IntPtr Buffer, ILuint BufferSize, ILenum DXTCFormat);

        /// <summary>
        /// Errors that occur in ILU and ILUT are also reported through ilGetError. ilGetError only returns something other than IL_NO_ERROR if detectable errors have occurred.
        /// </summary>
        /// <returns></returns>
        // ILAPI ILenum    ILAPIENTRY ilGetError(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILenum ilGetError();

        /// <summary>
        /// ilGetInteger returns the value of a selected mode.
        /// </summary>
        /// <param name="Mode">The mode value to be returned.</param>
        /// <returns></returns>
        // ILAPI ILint     ILAPIENTRY ilGetInteger(ILenum Mode);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILint ilGetInteger(ILenum Mode);

        /// <summary>
        /// ilGetIntegerv function returns the mode value in the Param parameter.
        /// </summary>
        /// <param name="Mode">The mode value to be returned.</param>
        /// <param name="Param">Array where the values are stored</param>
        // ILAPI ILvoid    ILAPIENTRY ilGetIntegerv(ILenum Mode, ILint *Param);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilGetIntegerv(ILenum Mode, out ILint Param);

        /// <summary>
        /// ilGetIntegerv function returns the mode value in the Param parameter.
        /// </summary>
        /// <param name="Mode">The mode value to be returned.</param>
        /// <param name="Param">Array where the values are stored</param>
        // ILAPI ILvoid    ILAPIENTRY ilGetIntegerv(ILenum Mode, ILint *Param);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilGetIntegerv(ILenum Mode, [Out] ILint[] Param);

        // ILAPI ILuint    ILAPIENTRY ilGetLumpPos(ILvoid);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILuint ilGetLumpPos();

        /// <summary>
        /// ilGetPalette returns an unsigned byte pointer to the current bound image's palette (if one exists) to allow direct access and modification to the contents of the palette.
        /// </summary>
        /// <returns></returns>
        // ILAPI ILubyte*  ILAPIENTRY ilGetPalette(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern IntPtr ilGetPalette();

        /// <summary>
        /// ilGetString returns a constant human-readable string describing the current OpenIL implementation.
        /// </summary>
        /// <param name="StringName">Describes the string to be retrieved.</param>
        /// <returns></returns>
        // ILAPI ILstring  ILAPIENTRY ilGetString(ILenum StringName);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILstring ilGetString(ILenum StringName);

        /// <summary>
        /// s behaviour, in order to optimize either speed, memory, compression or quality, depending wholly on what the user desires.
        /// </summary>
        /// <param name="Target">An enum indicating what behaviour of the library is to be controlled.</param>
        /// <param name="Mode">The desired behaviour.</param>
        // ILAPI ILvoid    ILAPIENTRY ilHint(ILenum Target, ILenum Mode);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilHint(ILenum Target, ILenum Mode);

        /// <summary>
        /// ilInit starts DevIL and must be called prior to using DevIL, or else DevIL will probably crash when you attempt to use it.
        /// </summary>
        // ILAPI ILvoid    ILAPIENTRY ilInit(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilInit();

        /// <summary>
        /// ilIsDisabled returns whether the mode indicated by Mode is disabled.
        /// </summary>
        /// <param name="Mode">Indicates an OpenIL mode</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilIsDisabled(ILenum Mode);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilIsDisabled(ILenum Mode);

        /// <summary>
        /// ilIsEnabled returns whether the mode indicated by Mode is enabled.
        /// </summary>
        /// <param name="Mode">Indicates an OpenIL mode</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilIsEnabled(ILenum Mode);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilIsEnabled(ILenum Mode);

        /// <summary>
        /// ilIsImage returns whether the image name in Image is a valid image in use. If the image name in Image is in use, ilIsImage returns IL_TRUE. If Image is 0, ilIsImage returns IL_FALSE, because the default image is a special image and is never returned by ilGenImages. If the image name has been deleted by ilDeleteImages or never generated byilGenImages, IL_FALSE is returned.
        /// </summary>
        /// <param name="Image">An image name.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilIsImage(ILuint Image);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilIsImage(ILuint Image);

        // ILAPI ILboolean ILAPIENTRY ilIsValid(ILenum Type, ILstring FileName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilIsValid(ILenum Type, ILstring FileName);

        // ILAPI ILboolean ILAPIENTRY ilIsValidF(ILenum Type, ILHANDLE File);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="File"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilIsValidF(ILenum Type, ILHANDLE File);

        // ILAPI ILboolean ILAPIENTRY ilIsValidL(ILenum Type, ILvoid *Lump, ILuint Size);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Lump"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilIsValidL(ILenum Type, IntPtr Lump, ILuint Size);

        // ILAPI ILboolean ILAPIENTRY ilIsValidL(ILenum Type, ILvoid *Lump, ILuint Size);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Lump"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilIsValidL(ILenum Type, byte[] Lump, ILuint Size);

        // ILAPI ILvoid    ILAPIENTRY ilKeyColour(ILclampf Red, ILclampf Green, ILclampf Blue, ILclampf Alpha);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Red"></param>
        /// <param name="Green"></param>
        /// <param name="Blue"></param>
        /// <param name="Alpha"></param>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilKeyColour(ILclampf Red, ILclampf Green, ILclampf Blue, ILclampf Alpha);

        /// <summary>
        /// ilLoad can be used much in the same way ilLoadImage is used, except with ilLoad, it is possible to force OpenIL to load a file as a specific image format, no matter what the extension.
        /// </summary>
        /// <param name="Type">Format Specification</param>
        /// <param name="FileName">File to load the image</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilLoad(ILenum Type, const ILstring FileName);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoad(ILenum Type, ILstring FileName);

        /// <summary>
        /// ilLoadF loads an image from a previously opened file
        /// </summary>
        /// <param name="Type">Image format</param>
        /// <param name="File">Pointer to a previous opened file</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilLoadF(ILenum Type, ILHANDLE File);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadF(ILenum Type, ILHANDLE File);

        /// <summary>
        /// The ilLoadImage function allows a general interface to the specific internal file-loading routines. The approach ilLoadImage takes toward determining image types is three-pronged. First, it finds the extension and checks to see if any user-registered functions (registered through ilRegisterLoad) match the extension. If nothing matches, it takes the extension and determines which function to call based on it. Lastly, it attempts to identify the image based on various image header verification functions, such as ilIsValidPngF. If all this checking fails, IL_FALSE is returned with no modification to the current bound image.
        /// </summary>
        /// <param name="FileName">Specifies which file to load an image from.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilLoadImage(const ILstring FileName);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadImage(ILstring FileName);

        /// <summary>
        /// ilLoadL loads an image from a memory lump
        /// </summary>
        /// <param name="Type">Image format</param>
        /// <param name="Lump">Lump Address</param>
        /// <param name="Size">Lump size</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilLoadL(ILenum Type, const ILvoid *Lump, ILuint Size);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadL(ILenum Type, IntPtr Lump, ILuint Size);

        /// <summary>
        /// ilLoadL loads an image from a memory lump
        /// </summary>
        /// <param name="Type">Image format</param>
        /// <param name="Lump">Lump Address</param>
        /// <param name="Size">Lump size</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilLoadL(ILenum Type, const ILvoid *Lump, ILuint Size);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadL(ILenum Type, byte[] Lump, ILuint Size);

        /// <summary>
        /// ilLoadPal simply loads a palette from the file specified by FileName into the current bound image's palette. If the current bound image is not of type IL_COLOR_INDEX, the palette is not used, but it is loaded nonetheless. ilLoadPal can load .col, Halo and Jasc PSP palette files.
        /// </summary>
        /// <param name="FileName">Filename to load the palette data from.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilLoadPal(const ILstring FileName);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadPal(ILstring FileName);

        // ILAPI ILvoid    ILAPIENTRY ilModAlpha( ILdouble AlphaValue );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlphaValue"></param>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilModAlpha(ILdouble AlphaValue);

        /// <summary>
        /// ilOriginFunc sets the origin to be used when loading all images, so that any image with a different origin will be flipped to have the set origin. This behaviour is actually disabled by default but can be enabled using ilEnable with the IL_ORIGIN_SET parameter.
        /// </summary>
        /// <param name="Mode">Specifies the origin to be used</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilOriginFunc(ILenum Mode);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern byte ilOriginFunc(ILenum Mode);

        /// <summary>
        /// The ilOverlayImage function copies the image named by Src onto the current bound image. XCoord, YCoord and ZCoord are allowed to be any number, even negative numbers, for if you want to start copying Src in the middle of it to the current image's left side. If the image named by Src has alpha components, then blending will occur, instead of just a simple overlay.
        /// </summary>
        /// <param name="Source">The image to copy.</param>
        /// <param name="XCoord">The starting x position of the current image to copy Src to.</param>
        /// <param name="YCoord">The starting y position of the current image to copy Src to.</param>
        /// <param name="ZCoord">The starting z position of the current image to copy Src to.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilOverlayImage(ILuint Source, ILint XCoord, ILint YCoord, ILint ZCoord);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilOverlayImage(ILuint Source, ILint XCoord, ILint YCoord, ILint ZCoord);

        /// <summary>
        /// ilPopAttrib pops the last pushed stack entry off the stack and copies the bits specified when pushed by ilPushAttrib to the previous set of states.
        /// </summary>
        // ILAPI ILvoid    ILAPIENTRY ilPopAttrib(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilPopAttrib();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Bits">Attribute bits to push.</param>
        // ILAPI ILvoid    ILAPIENTRY ilPushAttrib(ILuint Bits);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilPushAttrib(ILuint Bits);

        /// <summary>
        /// ilRegisterFormat tells OpenIL what format the current registered image is in. This function is to be used from within functions that have been registered via ilRegisterLoad
        /// </summary>
        /// <param name="Format">The format that the registered image is in.</param>
        // ILAPI ILvoid    ILAPIENTRY ilRegisterFormat(ILenum Format);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilRegisterFormat(ILenum Format);

        /// <summary>
        /// ilRegisterLoad allows the user to register functions for use by OpenIL, when loading unknown image types. The user can also override the default internal loading functions by passing their extension in Ext when using ilLoadImage. ilRegisterLoad allows the user to use their own loading functions while using OpenIL's capabilities, or to extend OpenIL when it does not support a specific image format
        /// </summary>
        /// <param name="Ext">Extension of the image type to load</param>
        /// <param name="Load">Pointer to a loading function</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilRegisterLoad(const ILstring Ext, IL_LOADPROC Load);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilRegisterLoad(ILstring Ext, IL_LOADPROC Load);

        /// <summary>
        /// ilRegisterMipNum tells OpenIL the number of mipmaps the current image has. The mipmaps can then be chosen and modified via ilActiveMipmap. This function is to be used from within functions that have been registered via ilRegisterLoad.
        /// </summary>
        /// <param name="Num">Number of mipmaps to create.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilRegisterMipNum(ILuint Num);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilRegisterMipNum(ILuint Num);

        /// <summary>
        /// ilRegisterNumImages tells OpenIL the number of images in the current image's animation chain. The "next" images can then be chosen and modified via ilActiveImage. This function is to be used from within functions that have been registered via ilRegisterLoad.
        /// </summary>
        /// <param name="Num">Number of images in the animation chain to create.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilRegisterNumImages(ILuint Num);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilRegisterNumImages(ILuint Num);

        /// <summary>
        /// ilRegisterFormat tells OpenIL what format the current registered image is in. This function is to be used from within functions that have been registered via ilRegisterLoad
        /// </summary>
        /// <param name="Origin">The new Origin of the image</param>
        // ILAPI ILvoid    ILAPIENTRY ilRegisterOrigin(ILenum Origin);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilRegisterOrigin(ILenum Origin);

        /// <summary>
        /// The ilRegisterPal function registers the current image's palette.
        /// </summary>
        /// <param name="Pal">Pointer to palette to be copied to the current image</param>
        /// <param name="Size">Size of Pal in bytes</param>
        /// <param name="Type">Type of the palette.</param>
        // ILAPI ILvoid    ILAPIENTRY ilRegisterPal(ILvoid *Pal, ILuint Size, ILenum Type);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilRegisterPal(IntPtr Pal, ILuint Size, ILenum Type);

        /// <summary>
        /// ilRegisterType tells OpenIL what datatype the current registered image uses. This function is to be used from within functions that have been registered via ilRegisterLoad.
        /// </summary>
        /// <param name="Ext">Extension of the image type to save</param>
        /// <param name="Save">Pointer to a saving function</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilRegisterSave(const ILstring Ext, IL_SAVEPROC Save);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilRegisterSave(ILstring Ext, IL_SAVEPROC Save);

        /// <summary>
        /// ilRegisterType tells OpenIL what datatype the current registered image uses. This function is to be used from within functions that have been registered via ilRegisterLoad.
        /// </summary>
        /// <param name="Type">The type the current image should be converted to.</param>
        // ILAPI ILvoid    ILAPIENTRY ilRegisterType(ILenum Type);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilRegisterType(ILenum Type);

        /// <summary>
        /// ilRemoveLoad removes a registered extension handler from the registered load functions list. Use this function when a new handler for an extension needs to be registered.
        /// </summary>
        /// <param name="Ext">Extension to remove (e.g. tga).</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilRemoveLoad(const ILstring Ext);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilRemoveLoad(ILstring Ext);

        /// <summary>
        /// ilRemoveSave removes a registered extension handler from the registered save functions list. Use this function when a new handler for an extension needs to be registered.
        /// </summary>
        /// <param name="Ext">Extension to remove (e.g. tga).</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilRemoveSave(const ILstring Ext);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilRemoveSave(ILstring Ext);

        // ILAPI ILvoid    ILAPIENTRY ilResetMemory(ILvoid);
        /// <summary>
        /// 
        /// </summary>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilResetMemory();

        /// <summary>
        /// ilResetRead resets the reading functions set by ilSetRead.
        /// </summary>
        // ILAPI ILvoid    ILAPIENTRY ilResetRead(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilResetRead();

        /// <summary>
        /// ilResetWrite resets the writing functions set by ilSetWrite.
        /// </summary>
        // ILAPI ILvoid    ILAPIENTRY ilResetWrite(ILvoid);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilResetWrite();

        /// <summary>
        /// ilSave can be used much in the same way ilSaveImage is used, except with ilSave, it is possible to force OpenIL to save a file as a specific image format, no matter what the extension.
        /// </summary>
        /// <param name="Type">Image format.</param>
        /// <param name="FileName">The filename of the file to save to.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilSave(ILenum Type, ILstring FileName);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilSave(ILenum Type, ILstring FileName);

        /// <summary>
        /// ilSaveF saves an image to a previously opened file
        /// </summary>
        /// <param name="Type">Image format</param>
        /// <param name="File">Pointer to a previous opened file</param>
        /// <returns></returns>
        // ILAPI ILuint    ILAPIENTRY ilSaveF(ILenum Type, ILHANDLE File);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILuint ilSaveF(ILenum Type, ILHANDLE File);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName">Specifies which file to save an image to</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilSaveImage(const ILstring FileName);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilSaveImage(ILstring FileName);

        /// <summary>
        /// ilSaveL saves an image to a memory lump
        /// </summary>
        /// <param name="Type">Image format</param>
        /// <param name="Lump">Lump Address</param>
        /// <param name="Size">Lump size</param>
        /// <returns></returns>
        // ILAPI ILuint    ILAPIENTRY ilSaveL(ILenum Type, ILvoid *Lump, ILuint Size);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILuint ilSaveL(ILenum Type, IntPtr Lump, ILuint Size);

        /// <summary>
        /// ilSaveL saves an image to a memory lump
        /// </summary>
        /// <param name="Type">Image format</param>
        /// <param name="Lump">Lump Address</param>
        /// <param name="Size">Lump size</param>
        /// <returns></returns>
        // ILAPI ILuint    ILAPIENTRY ilSaveL(ILenum Type, ILvoid *Lump, ILuint Size);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILuint ilSaveL(ILenum Type, byte[] Lump, ILuint Size);

        /// <summary>
        /// If the current bound image has a palette, ilSavePal saves the current image's palette to the file specified by FileName. Currently, OpenIL only supports saving to Paint Shop Pro .pal files.
        /// </summary>
        /// <param name="FileName">Filename to save the palette data to.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilSavePal(const ILstring FileName);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilSavePal(ILstring FileName);

        // ILAPI ILvoid    ILAPIENTRY ilSetAlpha( ILdouble AlphaValue );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlphaValue"></param>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilSetAlpha(ILdouble AlphaValue);

        /// <summary>
        /// ilSetData just updates the current bound image data (bound by ilBindImage) with new data of the same size. This way new memory does not have to be allocated, so transfers are much faster.
        /// </summary>
        /// <param name="Data">Specifies the new image data to update the image with.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilSetData(ILvoid *Data);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilSetData(IntPtr Data);

        /// <summary>
        /// ilSetDuration allows you to set how long to show the currently bound image. This function can also change the durations of individual images in animation chains.
        /// </summary>
        /// <param name="Duration">Number of milliseconds to play the currently bound image.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilSetDuration(ILuint Duration);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilSetDuration(ILuint Duration);

        /// <summary>
        /// The ilSetInteger function sets the value of a selected mode. it's the ilGetInteger's counterpart
        /// </summary>
        /// <param name="Mode">The mode value to be modified.</param>
        /// <param name="Param">The value to set the mode with.</param>
        // ILAPI ILvoid    ILAPIENTRY ilSetInteger(ILenum Mode, ILint Param);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilSetInteger(ILenum Mode, ILint Param);

        /// <summary>
        /// ilSetMemory was created to let DevIL users override the default memory allocation and deallocation functions present in DevIL. This support can be useful if you are using your own optimized memory handler or anything similar.
        /// </summary>
        /// <param name="AllocFunc">Specifies a function to override DevIL's allocation function.</param>
        /// <param name="FreeFunc">Specifies a function to override DevIL's deallocation function.</param>
        // ILAPI ILvoid    ILAPIENTRY ilSetMemory(mAlloc, mFree);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilSetMemory(mAlloc AllocFunc, mFree FreeFunc);

        /// <summary>
        /// ilCopyPixels has very simple behaviour. It simply copies a block of pixels from the Data buffer to the current image's data. XOff, YOff and ZOff can be used to skip a certain number of pixels in each respective direction. If XOff + Width, YOff + Height and/or ZOff + Depth is greater than the current image's width, height or depth, only the current image's width, height or depth number of pixels will be copied to the current image's data buffer.
        /// </summary>
        /// <param name="XOff">Where to begin copying pixels to in the x direction.</param>
        /// <param name="YOff">Where to begin copying pixels to in the y direction.</param>
        /// <param name="ZOff">Where to begin copying pixels to in the z direction.</param>
        /// <param name="Width">How many pixels to copy in the x direction.</param>
        /// <param name="Height">How many pixels to copy in the y direction.</param>
        /// <param name="Depth">How many pixels to copy in the z direction.</param>
        /// <param name="Format">The format the input is.</param>
        /// <param name="Type">The type the input is.</param>
        /// <param name="Data">User-defined buffer to copy the image data to.</param>
        // ILAPI ILvoid    ILAPIENTRY ilSetPixels(ILint XOff, ILint YOff, ILint ZOff, ILuint Width, ILuint Height, ILuint Depth, ILenum Format, ILenum Type, ILvoid *Data);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilSetPixels(ILint XOff, ILint YOff, ILint ZOff, ILuint Width, ILuint Height, ILuint Depth, ILuint Format, ILuint Type, IntPtr Data);

        /// <summary>
        ///  datatype ILHANDLE is passed to these functions when used, so any internal datatype used by the differing language (or file handle) can be used.
        /// </summary>
        /// <param name="Open">Pointer to a function to open a file.</param>
        /// <param name="Close">Pointer to a function to close a file.</param>
        /// <param name="Eof">Pointer to a function that returns IL_TRUE if the end of file is reached.</param>
        /// <param name="Getc">Pointer to a function to return one byte from a file.</param>
        /// <param name="Read">Pointer to a function to read multiple bytes from a file.</param>
        /// <param name="Seek">Pointer to a function to change position in a file.</param>
        /// <param name="Tell">Pointer to a function to report the position in a file.</param>
        // ILAPI ILvoid    ILAPIENTRY ilSetRead(fOpenRProc, fCloseRProc, fEofProc, fGetcProc, fReadProc, fSeekRProc, fTellRProc);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilSetRead(fOpenRProc Open, fCloseRProc Close, fEofProc Eof, fGetcProc Getc, fReadProc Read, fSeekRProc Seek, fTellRProc Tell);

        // ILAPI ILvoid    ILAPIENTRY ilSetString(ILenum Mode, const char *String);
        /// <summary>
        /// ilSetString gives DevIL users the option to set strings in certain file formats that have fields for strings, making DevIL highly customizable. Choose one of the acceptable parameters for Mode and specify any string you want. If the string is too long, it will be truncated when writing to the file.
        /// </summary>
        /// <param name="Mode">Specifies the string to be set.</param>
        /// <param name="str">String to use for setting a string field of a specified image format.</param>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilSetString(ILenum Mode, string str);

        /// <summary>
        /// ilSetWrite allows you to override the default DevIL saving functions with your own. You are virtually unlimited in how your functions work, as long as they have the same behaviour as DevIL's default saving functions. All the functions work on the ILHANDLE type, which is a just a void pointer.
        /// </summary>
        /// <param name="Open">Pointer to a function to open a file.</param>
        /// <param name="Close">Pointer to a function to close a file.</param>
        /// <param name="Putc">Pointer to a function to write one byte to a file.</param>
        /// <param name="Seek">Pointer to a function to change position in a file.</param>
        /// <param name="Tell">Pointer to a function to report the position in a file.</param>
        /// <param name="Write">Pointer to a function to write multiple bytes to a file.</param>
        // ILAPI ILvoid    ILAPIENTRY ilSetWrite(fOpenWProc, fCloseWProc, fPutcProc, fSeekWProc, fTellWProc, fWriteProc);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilSetWrite(fOpenWProc Open, fCloseWProc Close, fPutcProc Putc, fSeekWProc Seek, fTellWProc Tell, fWriteProc Write);

        // ILAPI ILvoid    ILAPIENTRY ilShutDown(ILvoid);
        /// <summary>
        /// 
        /// </summary>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern void ilShutDown();

        /// <summary>
        /// Any current image data is destroyed by ilTexImage and replaced by a new image with the attributes specified. The new image data has undefined values. To set the new image data to a certain value, use ilClearImage or ilClearImageTo.
        /// </summary>
        /// <param name="Width">Specifies the new image width. This cannot be 0.</param>
        /// <param name="Height">Specifies the new image height. This cannot be 0.</param>
        /// <param name="Depth">Specifies the new image depth. Anything greater than 1 will make the image 3d. This cannot be 0.</param>
        /// <param name="numChannels">Specifies the new channels. Common values are 3 and 4.</param>
        /// <param name="Format">Specifies the data format this image has. For a list of values this can be, see the See Also section.</param>
        /// <param name="Type">Specifies the data format this image has. For a list of values this can be, see the See Also section.</param>
        /// <param name="Data">Specifies data that should be copied to the new image. If this parameter is NULL, no data is copied, and the new image data consists of undefined values.</param>
        /// <returns></returns>
        // ILAPI ILboolean ILAPIENTRY ilTexImage(ILuint Width, ILuint Height, ILuint Depth, ILubyte numChannels, ILenum Format, ILenum Type, ILvoid *Data);
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilTexImage(ILuint Width, ILuint Height, ILuint Depth, ILubyte numChannels, ILenum Format, ILenum Type, IntPtr Data);

        // ILAPI ILenum    ILAPIENTRY ilTypeFromExt(const ILstring FileName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILenum ilTypeFromExt(ILstring FileName);

        // ILAPI ILboolean ILAPIENTRY ilTypeFunc(ILenum Mode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilTypeFunc(ILenum Mode);

        // ILAPI ILboolean ILAPIENTRY ilLoadData(const ILstring FileName, ILuint Width, ILuint Height, ILuint Depth, ILubyte Bpp);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Depth"></param>
        /// <param name="Bpp"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadData(ILstring FileName, ILuint Width, ILuint Height, ILuint Depth, ILubyte Bpp);

        // ILAPI ILboolean ILAPIENTRY ilLoadDataF(ILHANDLE File, ILuint Width, ILuint Height, ILuint Depth, ILubyte Bpp);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="File"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Depth"></param>
        /// <param name="Bpp"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadDataF(ILHANDLE File, ILuint Width, ILuint Height, ILuint Depth, ILubyte Bpp);

        // ILAPI ILboolean ILAPIENTRY ilLoadDataL(ILvoid *Lump, ILuint Size, ILuint Width, ILuint Height, ILuint Depth, ILubyte Bpp);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lump"></param>
        /// <param name="Size"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Depth"></param>
        /// <param name="Bpp"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadDataL(IntPtr Lump, ILuint Size, ILuint Width, ILuint Height, ILuint Depth, ILubyte Bpp);

        // ILAPI ILboolean ILAPIENTRY ilLoadDataL(ILvoid *Lump, ILuint Size, ILuint Width, ILuint Height, ILuint Depth, ILubyte Bpp);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lump"></param>
        /// <param name="Size"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Depth"></param>
        /// <param name="Bpp"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadDataL(byte[] Lump, ILuint Size, ILuint Width, ILuint Height, ILuint Depth, ILubyte Bpp);

        // ILAPI ILboolean ILAPIENTRY ilSaveData(const ILstring FileName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilSaveData(ILstring FileName);

        // ILAPI ILboolean ILAPIENTRY ilLoadFromJpegStruct(ILvoid* JpegDecompressorPtr);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="JpegDecompressorPtr"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilLoadFromJpegStruct(IntPtr JpegDecompressorPtr);

        // ILAPI ILboolean ILAPIENTRY ilSaveFromJpegStruct(ILvoid* JpegCompressorPtr);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="JpegCompressorPtr"></param>
        /// <returns></returns>
        [DllImport(DEVIL_NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
        public static extern ILboolean ilSaveFromJpegStruct(IntPtr JpegCompressorPtr);

        #endregion Externs
    }
}
