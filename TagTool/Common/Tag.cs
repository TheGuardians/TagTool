using System;
using System.Text;

namespace TagTool.Common
{
    /// <summary>
    /// Represents a magic number which looks like a string.
    /// </summary>
    public struct Tag : IComparable<Tag>, IBlamType
	{
		#region Tags

		/// <summary>
		/// The null tag representation.
		/// </summary>
		public static Tag NULL { get; } = new Tag("ÿÿÿÿ");

		public static Tag _FX_ { get; } = new Tag("<fx>");
		public static Tag ACHI { get; } = new Tag("achi");
		public static Tag ADLG { get; } = new Tag("adlg");
		public static Tag AIGL { get; } = new Tag("aigl");
		public static Tag ANT_ { get; } = new Tag("ant!");
		public static Tag ARGD { get; } = new Tag("argd");
		public static Tag ARMR { get; } = new Tag("armr");
		public static Tag ARMS { get; } = new Tag("arms");
		public static Tag BEAM { get; } = new Tag("beam");
		public static Tag BINK { get; } = new Tag("bink");
		public static Tag BIPD { get; } = new Tag("bipd");
		public static Tag BITM { get; } = new Tag("bitm");
		public static Tag BKEY { get; } = new Tag("bkey");
		public static Tag BLOC { get; } = new Tag("bloc");
		public static Tag BMP3 { get; } = new Tag("bmp3");
		public static Tag BSDT { get; } = new Tag("bsdt");
		public static Tag CDDF { get; } = new Tag("cddf");
		public static Tag CFGT { get; } = new Tag("cfgt");
		public static Tag CFXS { get; } = new Tag("cfxs");
		public static Tag CHAD { get; } = new Tag("chad");
		public static Tag CHAR { get; } = new Tag("char");
		public static Tag CHDT { get; } = new Tag("chdt");
		public static Tag CHGD { get; } = new Tag("chgd");
		public static Tag CHMT { get; } = new Tag("chmt");
		public static Tag CINE { get; } = new Tag("cine");
		public static Tag CISC { get; } = new Tag("cisc");
		public static Tag CLWD { get; } = new Tag("clwd");
		public static Tag CMOE { get; } = new Tag("cmoe");
		public static Tag CNTL { get; } = new Tag("cntl");
		public static Tag COLL { get; } = new Tag("coll");
		public static Tag COLO { get; } = new Tag("colo");
		public static Tag CPRL { get; } = new Tag("cprl");
		public static Tag CREA { get; } = new Tag("crea");
		public static Tag CRTE { get; } = new Tag("crte");
		public static Tag CTRL { get; } = new Tag("ctrl");
		public static Tag DCTR { get; } = new Tag("dctr");
		public static Tag DECS { get; } = new Tag("decs");
		public static Tag DRAW { get; } = new Tag("draw");
		public static Tag DRDF { get; } = new Tag("drdf");
		public static Tag DSRC { get; } = new Tag("dsrc");
		public static Tag EFFE { get; } = new Tag("effe");
		public static Tag EFFG { get; } = new Tag("effg");
		public static Tag EFSC { get; } = new Tag("efsc");
		public static Tag EQIP { get; } = new Tag("eqip");
		public static Tag FLCK { get; } = new Tag("flck");
		public static Tag FOOT { get; } = new Tag("foot");
		public static Tag FORG { get; } = new Tag("forg");
		public static Tag FORM { get; } = new Tag("form");
		public static Tag GFXT { get; } = new Tag("gfxt");
		public static Tag GINT { get; } = new Tag("gint");
		public static Tag GLPS { get; } = new Tag("glps");
		public static Tag GLVS { get; } = new Tag("glvs");
		public static Tag GOOF { get; } = new Tag("goof");
		public static Tag GPDT { get; } = new Tag("gpdt");
		public static Tag GRUP { get; } = new Tag("grup");
		public static Tag HLMT { get; } = new Tag("hlmt");
		public static Tag INPG { get; } = new Tag("inpg");
		public static Tag JMAD { get; } = new Tag("jmad");
		public static Tag JMRQ { get; } = new Tag("jmrq");
		public static Tag JPT_ { get; } = new Tag("jpt!");
		public static Tag LBSP { get; } = new Tag("Lbsp");
		public static Tag LENS { get; } = new Tag("lens");
		public static Tag LIGH { get; } = new Tag("ligh");
		public static Tag LSND { get; } = new Tag("lsnd");
		public static Tag LST3 { get; } = new Tag("lst3");
		public static Tag LSWD { get; } = new Tag("lswd");
		public static Tag LTVL { get; } = new Tag("ltvl");
		public static Tag MACH { get; } = new Tag("mach");
		public static Tag MATG { get; } = new Tag("matg");
		public static Tag MDL3 { get; } = new Tag("mdl3");
		public static Tag MDLG { get; } = new Tag("mdlg");
		public static Tag MFFN { get; } = new Tag("mffn");
		public static Tag MODE { get; } = new Tag("mode");
		public static Tag MULG { get; } = new Tag("mulg");
		public static Tag NCLT { get; } = new Tag("nclt");
		public static Tag OBJE { get; } = new Tag("obje");
		public static Tag PDM_ { get; } = new Tag("pdm!");
		public static Tag PECP { get; } = new Tag("pecp");
		public static Tag PERF { get; } = new Tag("perf");
		public static Tag PHMO { get; } = new Tag("phmo");
		public static Tag PIXL { get; } = new Tag("pixl");
		public static Tag PLAY { get; } = new Tag("play");
		public static Tag PMDF { get; } = new Tag("pmdf");
		public static Tag PMOV { get; } = new Tag("pmov");
		public static Tag PPHY { get; } = new Tag("pphy");
		public static Tag PROJ { get; } = new Tag("proj");
		public static Tag PRT3 { get; } = new Tag("prt3");
		public static Tag RASG { get; } = new Tag("rasg");
		public static Tag RM__ { get; } = new Tag("rm  ");
		public static Tag RMBK { get; } = new Tag("rmbk");
		public static Tag RMCS { get; } = new Tag("rmcs");
		public static Tag RMCT { get; } = new Tag("rmct");
		public static Tag RMD_ { get; } = new Tag("rmd ");
		public static Tag RMDF { get; } = new Tag("rmdf");
		public static Tag RMFL { get; } = new Tag("rmfl");
		public static Tag RMHG { get; } = new Tag("rmhg");
		public static Tag RMOP { get; } = new Tag("rmop");
		public static Tag RMSH { get; } = new Tag("rmsh");
		public static Tag RMSS { get; } = new Tag("rmss");
		public static Tag RMT2 { get; } = new Tag("rmt2");
		public static Tag RMTR { get; } = new Tag("rmtr");
		public static Tag RMW_ { get; } = new Tag("rmw ");
		public static Tag RMZO { get; } = new Tag("rmzo");
		public static Tag RWRD { get; } = new Tag("rwrd");
		public static Tag SBSP { get; } = new Tag("sbsp");
		public static Tag SCEN { get; } = new Tag("scen");
		public static Tag SCN3 { get; } = new Tag("scn3");
		public static Tag SCNR { get; } = new Tag("scnr");
		public static Tag SDDT { get; } = new Tag("sddt");
		public static Tag SEFC { get; } = new Tag("sefc");
		public static Tag SFX_ { get; } = new Tag("sfx+");
		public static Tag SGP_ { get; } = new Tag("sgp!");
		public static Tag SHIT { get; } = new Tag("shit");
		public static Tag SIIN { get; } = new Tag("siin");
		public static Tag SILY { get; } = new Tag("sily");
		public static Tag SKN3 { get; } = new Tag("skn3");
		public static Tag SKYA { get; } = new Tag("skya");
		public static Tag SLDT { get; } = new Tag("sLdT");
		public static Tag SMDT { get; } = new Tag("smdt");
		public static Tag SNCL { get; } = new Tag("sncl");
		public static Tag SND_ { get; } = new Tag("snd!");
		public static Tag SNDE { get; } = new Tag("snde");
		public static Tag SNMX { get; } = new Tag("snmx");
		public static Tag SPK_ { get; } = new Tag("spk!");
		public static Tag SQTM { get; } = new Tag("sqtm");
		public static Tag SSCE { get; } = new Tag("ssce");
		public static Tag STYL { get; } = new Tag("styl");
		public static Tag SUS_ { get; } = new Tag("sus!");
		public static Tag TERM { get; } = new Tag("term");
		public static Tag TRAK { get; } = new Tag("trak");
		public static Tag TRDF { get; } = new Tag("trdf");
		public static Tag TXT3 { get; } = new Tag("txt3");
		public static Tag UDLG { get; } = new Tag("udlg");
		public static Tag UGH_ { get; } = new Tag("ugh!");
		public static Tag UISE { get; } = new Tag("uise");
		public static Tag UNIC { get; } = new Tag("unic");
		public static Tag VEHI { get; } = new Tag("vehi");
		public static Tag VFSL { get; } = new Tag("vfsl");
		public static Tag VMDX { get; } = new Tag("vmdx");
		public static Tag VTSH { get; } = new Tag("vtsh");
		public static Tag WACD { get; } = new Tag("wacd");
		public static Tag WCLR { get; } = new Tag("wclr");
		public static Tag WEAP { get; } = new Tag("weap");
		public static Tag WEZR { get; } = new Tag("wezr");
		public static Tag WGAN { get; } = new Tag("wgan");
		public static Tag WGTZ { get; } = new Tag("wgtz");
		public static Tag WIGL { get; } = new Tag("wigl");
		public static Tag WIND { get; } = new Tag("wind");
		public static Tag WPOS { get; } = new Tag("wpos");
		public static Tag WROT { get; } = new Tag("wrot");
		public static Tag WSCL { get; } = new Tag("wscl");
		public static Tag WSPR { get; } = new Tag("wspr");
		public static Tag WTUV { get; } = new Tag("wtuv");
		public static Tag ZONE { get; } = new Tag("zone");
		#endregion

        /// <summary>
        /// Constructs a magic number from an integer.
        /// </summary>
        /// <param name="val">The integer.</param>
        public Tag(int val)
        {
            Value = val;
        }

        /// <summary>
        /// Constructs a magic number from a string.
        /// </summary>
        /// <param name="str">The string.</param>
        public Tag(string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            Value = 0;
            foreach (var b in bytes)
            {
                Value <<= 8;
                Value |= b;
            }
        }

        /// <summary>
        /// Constructs a magic number from a character array.
        /// </summary>
        /// <param name="input">The character array.</param>
        public Tag(char[] input)
        {
            var chars = new char[4] { ' ', ' ', ' ', ' ' };

            for (var i = 0; i < input.Length; i++)
                chars[i] = input[i];

            Value = 0;
            foreach (var c in chars)
            {
                Value <<= 8;
                Value |= c;
            }
        }

        /// <summary>
        /// Gets the value of the magic number as an integer. 
		/// THERE BE DRAGONS HERE: Do not set this manually outside of serialization.
        /// </summary>
        public int Value { get; set; }

		/// <summary>
		/// Converts the magic number into its string representation.
		/// </summary>
		/// <returns>The string that the magic number looks like.</returns>
		public override string ToString()
        {
            var i = 4;
            var chars = new char[4];
            var val = Value;
            while (val > 0)
            {
                i--;
                chars[i] = (char)(val & 0xFF);
                val >>= 8;
            }
            return (i < 4) ? new string(chars, i, chars.Length - i) : "";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tag))
                return false;
            var other = (Tag)obj;
            return (Value == other.Value);
        }

        public static bool operator ==(Tag a, Tag b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Tag a, Tag b)
        {
            return !(a == b);
        }

        public static bool operator ==(Tag a, string b)
        {
            return a == new Tag(b);
        }

        public static bool operator !=(Tag a, string b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public int CompareTo(Tag other)
        {
            return Value - other.Value;
        }

	}
}
