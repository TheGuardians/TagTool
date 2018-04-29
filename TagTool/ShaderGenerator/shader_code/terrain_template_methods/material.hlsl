#include "../helpers.hlsl"

void material_diffuse_only()
{


}

void material_diffuse_plus_specular()
{


}

void material_diffuse_off()
{


}

#ifdef flag_material_0_off
#define material_0_off material_off
#endif
#ifdef flag_material_1_off
#define material_1_off material_off
#endif
#ifdef flag_material_2_off
#define material_2_off material_off
#endif
#ifdef flag_material_3_off
#define material_3_off material_off
#endif

#ifdef flag_material_0_diffuse_only
#define material_0_diffuse_only material_diffuse_only
#endif
#ifdef flag_material_1_diffuse_only
#define material_1_diffuse_only material_diffuse_only
#endif
#ifdef flag_material_2_diffuse_only
#define material_2_diffuse_only material_diffuse_only
#endif
#ifdef flag_material_3_diffuse_only
#define material_3_diffuse_only material_diffuse_only
#endif

#ifdef flag_material_0_diffuse_plus_specular
#define material_0_diffuse_plus_specular material_diffuse_plus_specular
#endif
#ifdef flag_material_1_diffuse_plus_specular
#define material_1_diffuse_plus_specular material_diffuse_plus_specular
#endif
#ifdef flag_material_2_diffuse_plus_specular
#define material_2_diffuse_plus_specular material_diffuse_plus_specular
#endif
#ifdef flag_material_3_diffuse_plus_specular
#define material_3_diffuse_plus_specular material_diffuse_plus_specular
#endif