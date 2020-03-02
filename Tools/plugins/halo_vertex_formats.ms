-- Author: Beatthezombie 
-- License: GNU General Public License v3.0 http://www.gnu.org/licenses/gpl.txt

-- Halo Vertex Formats Prototype

struct rigid_vertex
(
	vertex_position  = point3 0 0 0,
	vertex_texcoord = point3 0 0 0,
	vertex_normal = point3 0 0 0,
	vertex_tangent = point3 0 0 0,
	vertex_binormal = point3 0 0 0,

	fn read stream =
	(
		vertex_position = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		vertex_texcoord = point3 (ReadFloat stream) (ReadFloat stream) 0
		vertex_normal = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		vertex_tangent = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		vertex_binormal = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
	),
	
	fn write stream =
	(

		WriteFloat stream vertex_position.x
		WriteFloat stream vertex_position.y
		WriteFloat stream vertex_position.z
		
		WriteFloat stream vertex_texcoord.x
		WriteFloat stream vertex_texcoord.y
		
		WriteFloat stream vertex_normal.x
		WriteFloat stream vertex_normal.y
		WriteFloat stream vertex_normal.z
		
		WriteFloat stream vertex_tangent.x
		WriteFloat stream vertex_tangent.y
		WriteFloat stream vertex_tangent.z
		
		WriteFloat stream vertex_binormal.x
		WriteFloat stream vertex_binormal.y
		WriteFloat stream vertex_binormal.z
	)
)

struct skinned_vertex
(
	vertex_position  = point3 0 0 0,
	vertex_texcoord = point3 0 0 0,
	vertex_normal = point3 0 0 0,
	vertex_tangent = point3 0 0 0,
	vertex_binormal = point3 0 0 0,
	vertex_node_1 = 0,
	vertex_node_2 = 0,
	vertex_node_3 = 0,
	vertex_node_4 = 0,
	vertex_weight_1 = 0,
	vertex_weight_2 = 0,
	vertex_weight_3 = 0,
	vertex_weight_4 = 0,

	fn read stream =
	(
		vertex_position = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		vertex_texcoord = point3 (ReadFloat stream) (ReadFloat stream) 0
		vertex_normal = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		vertex_tangent = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		vertex_binormal = point3 (ReadFloat stream) (ReadFloat stream) (ReadFloat stream)
		
		vertex_node_1 = ReadByte stream
		vertex_node_2 = ReadByte stream
		vertex_node_3 = ReadByte stream
		vertex_node_4 = ReadByte stream
		
		vertex_weight_1 = ReadFloat stream
		vertex_weight_2 = ReadFloat stream
		vertex_weight_3 = ReadFloat stream
		vertex_weight_4 = ReadFloat stream
	),
	
	fn write stream =
	(

		WriteFloat stream vertex_position.x
		WriteFloat stream vertex_position.y
		WriteFloat stream vertex_position.z
		
		WriteFloat stream vertex_texcoord.x
		WriteFloat stream vertex_texcoord.y
		
		WriteFloat stream vertex_normal.x
		WriteFloat stream vertex_normal.y
		WriteFloat stream vertex_normal.z
		
		WriteFloat stream vertex_tangent.x
		WriteFloat stream vertex_tangent.y
		WriteFloat stream vertex_tangent.z
		
		WriteFloat stream vertex_binormal.x
		WriteFloat stream vertex_binormal.y
		WriteFloat stream vertex_binormal.z
		
		WriteByte stream vertex_node_1
		WriteByte stream vertex_node_2
		WriteByte stream vertex_node_3
		WriteByte stream vertex_node_4
		
		WriteFloat stream vertex_weight_1
		WriteFloat stream vertex_weight_2
		WriteFloat stream vertex_weight_3
		WriteFloat stream vertex_weight_4
	)
)