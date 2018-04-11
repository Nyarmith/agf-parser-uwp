using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agf_parser_uwp
{
    public class GlobSetting
    {
        private static GlobSetting inst_;
        private GlobSetting() { }
        public static GlobSetting getInstance()
        {
            if(inst_ == null)
            {
                inst_ = new GlobSetting();
            }
            return inst_;
        }

        public int font_size = 1;
        public bool text_speech = false;
    }
}
