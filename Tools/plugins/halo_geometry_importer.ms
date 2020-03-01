-- Author: Beatthezombie 
-- License: GNU General Public License v3.0 http://www.gnu.org/licenses/gpl.txt

-- Halo Geometry Importer Prototype

include "halo_geometry_format.ms"

fn read_file filename =
(
	local f, halo_model = halo_geometry_format()
	if filename != undefined and doesFileExist filename == true then
	(
		try (fclose f) catch()
		f = fopen filename "rb"
		fseek f 0 #seek_set
		
		halo_model.read(f)
		
		print("Model name: " + halo_model.name)

		for item in halo_model.sh_order_3_red do print(item)
		
		for item in halo_model.materials do print(item.name)
		
		format "Last Read @ 0x%\n" (bit.intAsHex((ftell f) as integer))
		fclose f
	)
	else
	(
		format "Aborted\n"
	)
)
clearlistener()
read_file("D:\\3dsmax\\test.hgf")