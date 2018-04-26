
	
	
	A UPDB file can be generated for a shader by combining DebugData+ConstantData+ShaderData (in that order)
into a single file, disassembling using xsd.exe, and then reassembling with the /XZi option using
psa.exe or vsa.exe (vsa and psa can both be used to assemble either type of shader, not sure why both exist)

	The UPDB is in xml format, and an xml serializer can be used to load all it's data into a C# object.
The data contained in the UPDB is useful because it contains Interpolators and Constants.

	These Interpolators and Constants can also be obtained from ConstantData. It's probably best to get the
info that way for two reasons: speed, and not having to rely on Microcucks tools. However, ConstantData still
needs to be reversed pretty much from the bottom up before we can start decoding that instead of using the UPDB.

	I will attempt to load data from the UPDB in a way that it could easily be swapped with ConstantData decoding
in the future. I just don't feel like bit fiddling anymore for the time being T.T