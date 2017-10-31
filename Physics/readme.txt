colltest + phmotest : usage, information and content creation guide:
-------------------------------------------

colltest:
--------

colltest <filepath>|<dirpath> <index>|<new> [force]

The first argument could be either a path to a file with the extension: '.model_collision_geometry' 
or a directory full of files with the extension.  

The second argument could be either the index of an existing tag in the tag.dat to overwrite the tag
 or 'new' to create a new tag. 

If the tag to be overwritten is not of type 'coll' then the third optional argument 'force' must be
specified or the operation is not completed.

File information:
The file to import (*.model_collision_geometry) is a collision tag produced by the Halo 1 CE Tool 
program. This tag file can be created by supplying Tool with a JMS model (*.jms). The JMS model 
format for Halo 1 has support for vertex weighting and uv coordinates however these features are
not relevent to collision meshes and are not discussed. Materials, nodes and markers can also be 
defined in the JMS file. The materials are useful if the artist wishes to define areas of the mesh
to behave differently on impact. One node is used to mark the origin and anchor the object. Markers
are not relevant to coll tags. 

The mesh must be closed such that each edge is adjacent to exactly two faces. If degenerate edges
or holes exist in the mesh then the Halo 1 CE Tool will not produce the tag. It is fine for edges
or vertices to be duplicates as they are detected and removed by Tool. The mesh can be concave.

Multiple closed meshes in a single object is not well supported. During testing it was found that
 collision did not behave correctly for all separated meshes. This could be attributed to 
disconnects in the adjacency graph produced for the model. For this reason the option to specify a
directory of many '.model_collision_geometry' files for import has been implemented. This feature
currently loads each '.model_collision_geometry' file as a separate BSP however only one BSP will
be collidable. 

File creation:
The Blender addon: 'JMSExporter.py' is provided to export objects from a Blender scene to the JMS 
model format (See- Installing Blender addons). The addon can be used by navigating to 'File>Export>
Halo 1 CE JMS Exporter collision (.jms)'. Select a filename that does not exist as a directory name
in the destination. This is important because the exporter will create a folder hierarchy which will
 automate most of your interaction with Halo 1 CE Tool. The file name should also have underscores
in place of spaces as a batch script will be used later. The directory created should be moved to
'<Halo 1 CE Tool directory>/data/' and the batch file 'generate.bat' inside the directory is to be 
executed. The batch file was generated during exporting and will invoke Halo 1 CE Tool and collect
the output in the same directory as the batch file. The output should be enumerated from 000, 
ascending by 1. If a file is missing then an issue was encountered with the model. Refer to the
batch output for this. 

The user can specify whether all objects in the scene or selected objects are exported 
by using the checkbox in the left pane during the export process. 


Installing Halo 1 CE Tool:
---------------------------
Download HEK setup: http://hce.halomaps.org/index.cfm?fid=411 . Extract the contents of the '.exe' 
using winzip through the winzip context menu. Download sound libraries:
http://hce.halomaps.org/index.cfm?fid=1632 and extract to the directory containing 'tool.exe'. This 
directory will also contain two subdirectories: 'data' and 'tags'. The 'data' subdirectory is the
location that Tool will be expecting data files for compilation into tags. The compiled tags are
normally put into the 'tags' subdirectory.


==============================

phmotest:
-----------

phmotest <filepath> <index>|<new> [force]

The first argument is a path to a JSON file (.json) which contains an array of physics objects each 
with a type and type dependent properties.

The second argument could be either the index of an existing tag in the tag.dat to overwrite the tag
 or 'new' to create a new tag. 

If the tag to be overwritten is not of type 'phmo' then the third optional argument 'force' must be
specified or the operation is not completed.

File information:
The JSON file (.json) contains an array of physics objects each with a type and type dependent 
properties. The 'Polyhedron' is the only physics object currently supported. Properties are 
contained in the 'Data' property are dependent on the type of physics object. The properties
'Friction', 'Restitiution' and 'Mass' apply directly to the polyhedron. They are usually 
predetermined but have no noticeable effect currently as polyhedra are given a 'Keyframed' motion 
type. The 'Planes' property is an array of size 4 arrays. Each element of the array is a plane 
equation of the form i,j,k,distance. Each plane defines a half-space for the polyhedron. The 
polyhedron is a combination of at least four linearly independent planes. For this reason polyhedra 
can only represent convex hulls. The 'Vertices' contain an array of size 3 arrays. Each element of
the array is a vertex x,y,z. Each vertex represents the point where 3 or more planes intersect. The 
properties 'Center' and 'Extents' are used to define an axis aligned bounding box as part of a
bounding volume optimisation. 'Center' is the median-point, and 'Extents' are the magnitudes on the 
x,y,z axis. If any other object is not at least within the bounding volume then it is skipped 
during broad-phase. 

File creation:
The Blender addon: 'jsonPhmoExporter.py' is provided to export objects from a Blender scene to the
json file format (See- Installing Blender addons). The addon can be used by navigating to
'File > Export > Halo Online Tag Tool phmotest json exporter (.json)'. The objects exported must 
be convex and have closed meshes. If the mesh is not closed then the convex hull produced could be 
unbound. Currently performance of phmo is linear in the number of polyhedra. It is best to use as 
few polyhedra as possible until near logarithmic performance can be achieved. 

The user can specify whether all objects in the scene or selected objects are exported 
by using the checkbox in the left pane during the export process. 

The Blender addon: 'vtkTetraImport.py' is also provided for experimental purposes. It will import a 
VTK (.vtk) that contains a set of tetrahedra definitions by their extreme points. The VTK file 
format appears to have support for convex hulls with more points than tetrahedra however only 
tetrahedra are supported by the addon. This can be used to import the output of Tetgen into Blender.

The program- Tetgen is provided in 'tetgen.zip' (see accompanied readme for licensing) to convert a 
closed concave mesh into non-overlapping polyhedra. Usage is: 'tetgen -pYk <filepath>' , where 
filepath should reference a PLY model file (.ply). This model file can be exported from Blender
under 'File > Export > Stanford (.ply)'. The flags p, Y and k are described in the accompanied 
readme however they simply request a tetrahedralisation of the input mesh to a VTK (.vtk) file. The 
output file (.vtk) can be imported into Blender using the 'vtkTetraImport.py' addon provided.

Installing Blender addons:
---------------------------
Install the addon by moving the python script to the '<blender version>/scripts/addons/' directory 
of your Blender installation. Enable the addon in Blender: File>User Preferences>Addons , locating
and activating it in the list of addons. Save user settings.