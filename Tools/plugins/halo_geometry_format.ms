-- Author: Beatthezombie 
-- License: GNU General Public License v3.0 http://www.gnu.org/licenses/gpl.txt

--
-- Halo Geometry Format Definitions
--

global string_length = 0x40
global import_scale_factor = 100.0
global export_scale_factor = 1.0 / import_scale_factor

--
-- Helper functions to parse the file
--

fn ReadFixedLengthString stream =
(
    position = ftell stream
    str = ReadString stream
    new_position = position + string_length
    fseek stream new_position #seek_set
    str
)

fn WriteFixedLengthString stream str= 
(
	position = ftell stream
    WriteString stream str
    new_position = position + string_length
    fseek stream new_position #seek_set
)

struct tag_block
(
    count = 0,
    offset = 0,

    fn read stream =
    (
        count = ReadLong stream
        offset = ReadLong stream
        ReadLong stream
    ),

    fn write stream =
    (
        WriteLong stream count
        WriteLong stream offset
        WriteLong stream 0
    )
)

struct halo_geometry_format_header
(
    signature = 0,
    version = 0,
    flags = 0,
    content_flags = 0,
	offset = 0x14,

    fn read f = 
    (
        signature = ReadLong f #unsigned
        version = ReadLong f 
        flags = ReadLong f 
        content_flags = ReadLong f
		offset = ReadLong f
    ),

    fn write f =
    (
        WriteLong f signature #unsigned
        WriteLong f version
        WriteLong f flags
        WriteLong f content_flags
		WriteLong f offset
    )
)

struct halo_material
(
	name = "",
	
	fn read stream = 
	(
		name = ReadFixedLengthString stream
	),
	
	fn write stream =
	(
		WriteFixedLengthString stream name
	)
)

struct halo_node
(
	name = "",
	parent_node_index =-1,
	first_child_node_index = -1,
	next_sibling_node_index = -1,
	node_position = point3 0 0 0,
	node_rotation = quat 0 0 0 -1,
	node_scale = 1.0,
	
	fn read stream =
	(
		name = ReadFixedLengthString stream
		parent_node_index = ReadShort stream
		first_child_node_index = ReadShort stream
		next_sibling_node_index = ReadShort stream
		ReadShort stream
		node_position = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		node_rotation = quat (ReadFloat stream) (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		uniform_scale = ReadFloat stream
		node_scale = point3 uniform_scale uniform_scale uniform_scale
		
	),
	
	fn write stream =
	(
		
		WriteFixedLengthString stream name
		WriteShort stream parent_node_index
		WriteShort stream first_child_node_index
		WriteShort stream next_sibling_node_index
		WriteShort stream 0
		WriteFloat stream node_position.x
		WriteFloat stream node_position.y
		WriteFloat stream node_position.z
		WriteFloat stream node_rotation.x
		WriteFloat stream node_rotation.y
		WriteFloat stream node_rotation.z
		WriteFloat stream node_rotation.w
		WriteFloat stream node_scale.x -- assume uniform scale
		
	)
)

struct light_probe
(
	probe_position = point3 0 0 0,
	coefficients = #(),
	
	fn read stream =
	(
		probe_position	= point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		for i = 1 to 81 do coefficients[i] = ReadFloat stream
	),
	
	fn write stream =
	(
		WriteFloat stream probe_position.x
		WriteFloat stream probe_position.y
		WriteFloat stream probe_position.z
		for i = 1 to 81 do  WriteFloat stream coefficients[i]
	)
)

struct bounding_sphere
(
	plane_normal = point3 0 0 0,
	plane_distance = 0.0,
	sphere_position = point3 0 0 0,
	radius = 1.0,
	node_indices = #(),
	node_weights = #(),
	
	fn read stream =
	(
		plane_normal	= point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		plane_distance = ReadFloat stream
		sphere_position	= point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		radius = ReadFloat stream
		
		for i = 1 to 4 do node_indices[i] = ReadByte stream #signed
		sum_weights = 0
		for i = 1 to 3 do
		(
			node_weights[i] = ReadFloat stream
			sum_weights += node_weights[i] 
		)			
		node_weights[4] = 1.0 - sum_weights
	),
	
	fn write stream =
	(
		WriteFloat stream plane_normal.x
		WriteFloat stream plane_normal.y
		WriteFloat stream plane_normal.z
		WriteFloat stream plane_distance
		WriteFloat stream sphere_position.x
		WriteFloat stream sphere_position.y
		WriteFloat stream sphere_position.z
		WriteFloat stream radius
		for i = 1 to 4 do WriteByte stream node_indices[i] #signed
		for i = 1 to 3 do WriteFloat stream node_weights[i] 
	)
)

struct halo_geometry_format
(
	header = halo_geometry_format_header(),
	name = "",
    materials =#(),
    nodes = #(),
    meshes = #(),
    sh_order_3_red = #(),
    sh_order_3_green = #(),
    sh_order_3_blue = #(),
    light_probes = #(),
    bounding_spheres = #(),

    fn read stream =
    (
		header.read(stream)
		
		-- seek to the begininig of the geometry struct
		fseek stream header.offset #seek_set
		
		name = ReadFixedLengthString stream
		
		material_block = tag_block()
		nodes_block = tag_block()
		meshes_block = tag_block()
		light_probes_block = tag_block()
		bounding_spheres_block = tag_block()
		
        material_block.read(stream)
        nodes_block.read(stream)
        meshes_block.read(stream)

        for i = 1 to 16 do sh_order_3_red[i] = ReadFloat stream
        for i = 1 to 16 do sh_order_3_green[i] = ReadFloat stream
        for i = 1 to 16 do sh_order_3_blue[i] = ReadFloat stream
			
        light_probes_block.read(stream)
        bounding_spheres_block.read(stream)
		-- parse materials
		fseek stream material_block.offset #seek_set

		for i = 1 to material_block.count do
		(
			geometry_material = halo_material()
			geometry_material.read(stream)
			new_material = StandardMaterial()
			new_material.name = geometry_material.name
			materials[i] = new_material
		)
		-- parse nodes
		fseek stream nodes_block.offset #seek_set

		for i = 1 to nodes_block.count do
		(
			geometry_node = halo_node()
			geometry_node.read(stream)
			new_bone = undefined
			if geometry_node.parent_node_index == -1 then
			(
				new_bone = bone pos:geometry_node.node_position
				new_bone.position *= import_scale_factor
				new_bone.name = geometry_node.name
				new_bone.rotation = geometry_node.node_rotation
				new_bone.scale = geometry_node.node_scale
			)
			else
			(
				
				parent_bone = nodes[geometry_node.parent_node_index + 1] -- base 0 to base 1 index
				in coordsys parent_bone
				(
					new_bone = bone pos:geometry_node.node_position
					new_bone.position *= import_scale_factor
					new_bone.name = geometry_node.name
					new_bone.rotation = geometry_node.node_rotation
					new_bone.scale = geometry_node.node_scale
					new_bone.parent = parent_bone
				)
			)
			
			nodes[i] = new_bone
		)
		
		--parse light probes
		
		fseek stream light_probes_block.offset #seek_set
		
		for i = 1 to light_probes_block.count do
		(
			probe = light_probe()
			probe.read(stream)
			light_probes[i] = probe
		)
		
		--parse bounding spheres
		
		fseek stream bounding_spheres_block.offset #seek_set
		
		for i = 1 to bounding_spheres_block.count do
		(
			bs = bounding_sphere()
			bs.read(stream)

			attached_node = nodes[bs.node_indices[1] + 1] -- base 0 to base 1 index
			in coordsys attached_node
			(
				new_bs = sphere pos:bs.sphere_position
				new_bs.radius = bs.radius
				new_bs.radius *= import_scale_factor
				new_bs.parent = attached_node
				new_bs.name = "bounding_sphere_" + (i as string)
				bounding_spheres[i] = new_bs
			)
		)
		
    )

)

