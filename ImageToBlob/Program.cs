﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace ImageToBlob
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileText = string.Empty;
            var filePath = @"C:\Users\pjha\AppData\Local\Packages\Microsoft.SkypeApp_kzf8qxf38zg5c\LocalState\Downloads\start_bmp.txt";
            //using (var file in File.OpenRead(filePath))
            //{

                fileText = File.ReadAllText(filePath).Replace(@"\","");
            //}
                //var imageString = ConvertImagetoBlob();
            //LoadImage().Save(@"File.ico");
            LoadImage(fileText).Save(@"File.bmp");
            Console.Write("File Saved");
            Console.Read();
            
        }

        static string ConvertImagetoBlob()
        {
            string outputText = "";
            var imageStream = File.Open(@"D:\Javra\Project\ImportExcel\ImageToBlob\Image\Coffee.png", FileMode.Open);
           
            byte[] buffer;
            using (imageStream)
            {
                buffer = new byte[imageStream.Length];


                imageStream.Read(buffer, 0, (int)imageStream.Length);



            }
            var outText = Convert.ToBase64String(buffer);
          
            return outText;
        }
    
        static Image LoadImage(string Text)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            //byte[] bytes = Convert.FromBase64String(@"AAABAAEAICAAAAEAIACoEAAAFgAAACgAAAAgAAAAQAAAAAEAIAAAAAAAABAAAAAAAAAAAAAAAAAAAAAAAADY6ez\/2Ojr\/9jn6f\/W4OH\/yc7J\/7Kzrf+hoZv\/nqCa\/62sqv+xr6v\/pKKe\/5GRjf+SkIz\/j5OQ\/4+Wlv+SmZn\/kpma\/5Gam\/+SnJ7\/laCi\/5umqP+ms7X\/tcPG\/8XU1\/\/S4uX\/1+fq\/9jp7P\/Y6ez\/2Ons\/9jp7P\/Y6ez\/2Ons\/9jp7P\/Y5ef\/zdTT\/7Kzr\/+Jh4P\/cWpo\/2VdXf9jXl7\/bmlp\/5CNi\/+zsa\/\/yMjF\/8zNy\/\/IwLv\/wri5\/7+7yf\/Bw87\/xcjK\/8jLyv\/IzMz\/v8PE\/7e8vf+vtbb\/pq6v\/5+pqv+jr7H\/tsTH\/87e4f\/Y6ez\/2ert\/9jp7P\/Y6ez\/2Ons\/9Hd3v+tsK7\/cm5q\/0pAQP9INTr\/SzU8\/0Y0Ov9HNTr\/ioKD\/87My\/\/x8fD\/9ff4\/+7Xy\/\/ausD\/ybji\/9jX8f\/n5+z\/7u3o\/+jn5v\/W1dT\/0M\/O\/8nKyP+9v77\/r7Kz\/6itrv+lra7\/rLe6\/8fW2f\/V5un\/2ert\/9jq7f\/Y6ez\/x9LU\/4eMjP86ODj\/NiYs\/1Q3P\/9lREz\/Wz1G\/1M0PP+jlpj\/4+Pi\/\/Hy8f\/z9fb\/7Me0\/8eVoP+fh87\/vrzq\/9\/h6f\/q6uL\/29va\/8LCwP\/Y19X\/8fHv\/\/v7+\/\/+\/v7\/7Ozr\/7\/AwP+VmZn\/qLK0\/8jW1\/\/Z6On\/2Obo\/9jp7P+\/ysz\/fYGC\/zs3OP9GNzv\/XEFG\/2ZFS\/9gQEj\/XEBD\/6GWlv\/b2tn\/6+vr\/\/Pz8\/\/zxKr\/w4KQ\/4Nhvf+al97\/0NLj\/+Pi2f\/CwcH\/jYyL\/7Oysf\/c29r\/6uno\/\/z8\/P\/9\/f3\/8vPy\/9PT0v+RkI7\/srOu\/9XX0f\/W19L\/2Ons\/7nGx\/95fn\/\/QTs9\/1NAQv9lRUn\/akVL\/2RASP9gQkf\/mIuM\/87My\/\/m5ub\/8PDv\/\/TYyf\/TrLX\/ppHI\/7y64\/\/j5fD\/09TQ\/46Njf9gXl3\/joyM\/8nIx\/\/n5uX\/7Ovq\/\/n5+P\/\/\/\/\/\/3dzb\/2ppZv+AfXj\/sq6p\/8TBvf\/Y6ez\/tsXF\/3l+fv9IP0H\/XkVI\/21ITv9vRU3\/aEFJ\/2NBSP+KeXv\/u7e2\/9\/e3v\/r6+r\/8uvn\/+LY2v\/Nx9X\/4+Pr\/\/Hv8P\/Gw8L\/cnJz\/0NDRf96eXr\/xcTD\/+3s6\/\/h4N\/\/8PDw\/\/j4+P\/Lysr\/REND\/0xJR\/+HhH\/\/rqyo\/9jo6\/+4xsb\/fH9\/\/0w\/Qf9oSVD\/c0tT\/3NIUP9sREz\/Z0FH\/3tmaP+lnZ3\/0s\/O\/+Xj4v\/t7u7\/5ebl\/9ra1f\/q6uf\/7uTe\/8q7tv+Ee3n\/SEVG\/4iBf\/\/TzMr\/7uzr\/9zb2v\/r6+r\/5OTk\/6Wjpf8rKS7\/Liss\/2xrZv+goJv\/2Ojr\/77IyP+Af3\/\/Sjo\/\/25OVf95UFb\/d0xS\/3FHUP9pQEj\/dlxg\/5KHhv+1sbD\/19TT\/9\/f3v\/Z2dj\/0dDO\/9LQzf\/u4df\/5si5\/7KJef+HZlf\/wp+Q\/+jRx\/\/Y1tb\/0tHQ\/+rp6f\/BwMD\/XVlb\/ykkLP83NDb\/bm9r\/6Oln\/\/X5ef\/sbWz\/3Vubv9SPkT\/d1NZ\/39TWv96TVX\/dElR\/2xBSv99X2P\/jn+A\/5aSkf+gm5v\/vbu6\/9bU0f\/c1tL\/xsO9\/7HAxv+Wrb7\/fo6f\/4SIkP\/DuLH\/6NnM\/+DZ0\/\/c29n\/u7m5\/3l2d\/82MTT\/JyAl\/z87PP92dXL\/qKql\/9Xi5P+kpqP\/bmRi\/11HTP9\/V13\/hFVd\/35PWP95TFT\/cURM\/4FfY\/+TgoP\/mJWU\/4eEg\/+ioZ\/\/v7+9\/7vBxP+EpLf\/V5fB\/zyPyf85isb\/TZDD\/3enxP+pvMP\/ycO+\/7SwsP94cnT\/RTxB\/y0kKv8nICL\/RkFB\/399ev+vr6r\/0uDi\/56hoP9wZWP\/alFU\/4RaYf+IVl7\/g1Ja\/31PV\/92R07\/fVdZ\/5N8fP+lm5z\/kImJ\/5WNjP+QlZv\/apKs\/yiBwP8VgM\/\/FoTY\/xmH2\/8Vhtr\/GoDK\/0qKt\/+Bj5r\/X1pd\/0I1Ov83JzD\/NCgv\/ygiJP9NSkj\/iYeC\/7a1r\/\/P3d\/\/m52c\/3RmZv93Wl\/\/iltk\/4tXX\/+GU1r\/gVBX\/3lJUP94S1H\/eVBW\/3pVWv97WF3\/fmBj\/15sgv8rfLf\/J4\/d\/ymS5P8qkeH\/KpDg\/yqR4v8nk+b\/J4fP\/yplkf8vMDr\/Nigs\/zosMv82KTD\/KCAl\/1JQUP+RkY3\/vLu3\/8zZ2\/+YmZj\/eGZn\/4JfZv+QXGb\/kFpi\/4pVW\/+IWGD\/kXqC\/5iSmP+XmZ7\/kJKX\/4qLkP9pcn3\/QnKb\/yqP2\/8xmer\/M5rq\/zOZ6P8zmOf\/M5no\/zSc7v8slef\/IHW2\/x8tP\/8xJSj\/Pi0w\/zkqL\/8pHyT\/WlZX\/5yal\/\/Ewr7\/zNvd\/5aVlf96ZGf\/jGVt\/5ZgZ\/+TWmL\/j1dg\/5Ztdf+xvMH\/vd7g\/7zk5v+z3+H\/qNPV\/2absv87isH\/OqX3\/zuh8v86n+\/\/OZ\/v\/zmf7\/85n+\/\/O6Hx\/zmj9v8xktz\/Iklk\/zU0Ov9DMjP\/Oywu\/ykgIv9iXVz\/p6Of\/8zIwv\/P3d7\/lJGS\/3tjZv+VbnT\/m2Vq\/5RZYP+UXmf\/poqS\/8vt7v\/S\/\/\/\/zf\/\/\/8f\/\/\/+++\/v\/bbnY\/z2a2f9ErPz\/Qqj5\/0Gn+P9Apvf\/QKb3\/0Cm9\/9Bpvf\/Qav+\/z2j7f8tbpL\/Qldj\/01ERv8+LC7\/KSIi\/2lmYv+vrab\/0M3G\/8vV0\/+QjYn\/fWRl\/6F0e\/+gZ2\/\/mFtj\/5tpcv+xnaX\/0PDx\/9T\/\/\/\/P\/\/\/\/yv7+\/772+f9ns9b\/N5TS\/0Sn7\/9Grfv\/SK\/+\/0iv\/f9HrP3\/Rqz9\/0es\/v9Isf\/\/Razx\/zeApv9ajJX\/YXBv\/z8sMP8tIyX\/cmtq\/7e0sP\/R0sz\/u8G6\/4uDf\/+DZmf\/qXl\/\/6RocP+bXGP\/oXF2\/7irrv\/U8vP\/0fb3\/8jq6\/\/E5uf\/ut\/h\/1yat\/8db6L\/HnSv\/yZ\/v\/84muL\/R676\/020\/\/9NtP\/\/TbT\/\/0+5\/\/9MtPL\/PYep\/2Wdp\/9rf4D\/Qy4y\/zIoK\/92cnD\/urmz\/9LUzv+vsqv\/iX17\/4hoav+ufYH\/qWpw\/59cY\/+md3v\/vra3\/9j09P\/U9fb\/y+nq\/8jk5f\/A3uH\/XpWt\/xJfi\/8DWIz\/ClqP\/x10sf8yktb\/Qqbt\/0iu9v9Msvr\/SLH4\/0Om4v9LjKb\/b52m\/296f\/9HLjP\/Nywv\/3t5dP+9vbX\/1dXN\/6quqf+KfHz\/jWtu\/7OAg\/+ubnH\/pF9l\/6x7gv\/Fv8L\/3PX2\/93\/\/\/\/Y+\/v\/1fr7\/871+P95rsL\/JnOc\/wBflv8AVYr\/B1iM\/xZrpf8oh8n\/NZjd\/zqf5P8ylNb\/M4i7\/2egrf9+n6T\/bm51\/0otM\/89LjH\/g396\/8TDu\/\/a1tD\/pqij\/418ff+VbnL\/uIGF\/7Jvdf+rYWn\/tYGI\/8zGyv\/f9\/f\/4f\/+\/978\/P\/c\/P7\/1vz+\/6TS2\/9PkrD\/Al+U\/wNknP8KZZ7\/E22n\/xt5tv8efLv\/F3Sz\/xdnmv84dpH\/ldHV\/5Cusv9pY2n\/Si8z\/0MwNv+MhYP\/ysvF\/9re2\/+iop3\/kX5\/\/51zeP+9gon\/tG93\/69jbP+8iI\/\/1dDT\/+L4+P\/c8\/L\/1Obn\/9Hk5\/\/N5uj\/vtzf\/4Owv\/8xdpv\/DWqh\/wxvrf8UeLf\/F3u7\/xR2tf8VZpr\/N3GR\/3ikr\/+w7O3\/lbC0\/2dcYv9LMDT\/SDk9\/5ONjP\/O0s\/\/2OTl\/56cmP+WgIH\/pHZ8\/8CBif+3cHj\/smhv\/8GPlv\/d19r\/6Pn6\/+Dz8\/\/Y6Oj\/1eXm\/9Hm5\/\/S6ur\/utnd\/4Syw\/9HjrP\/L4O0\/ymDuP8rhLf\/Noaz\/0+Nqv99qrf\/r9rc\/775+P+Xqq7\/Z1Vb\/0wzNf9NQ0P\/mZaT\/9DX1f\/X5uj\/l5SR\/5iBgv+ren\/\/w3+H\/71zev+2bnX\/xZac\/+Lb3\/\/u+vr\/7P\/\/\/+j9\/f\/l\/f3\/4f39\/+D9\/f\/g\/v7\/0vT2\/6HL1v94rcL\/Yp64\/2Sftv9+rbv\/p9HW\/8Ht7f\/G+Pj\/xv39\/5mip\/9oT1X\/TjY3\/1RLSf+enZf\/0tnW\/9fl5\/+LiYf\/mYGD\/7WAhf\/Mg4r\/xXR9\/7xyef\/JnqL\/5eLj\/+34+P\/p+Pj\/5PX1\/+L29f\/g9vX\/3vX1\/9339\/\/b+fn\/0fP0\/8nu8f\/E7O\/\/w+zv\/8Xu8P\/K9PT\/zfn5\/839\/f\/K\/v7\/maCj\/2lOUf9TOTv\/WlJS\/6KhnP\/V2tb\/2OXm\/4R\/e\/+dgoT\/uoaM\/8mDiv+9cnf\/unN5\/86kqf\/r6er\/6vP0\/+Ht7f\/c6Oj\/3Onp\/9jp6f\/W5+f\/1ebm\/9Xo6P\/S6un\/0+vq\/9Pr6v\/R6+v\/z+vq\/8zp6P\/K6ur\/yvLz\/8v5+\/+Xmp7\/ZkpN\/1Q5PP9hU1b\/p6Sh\/9Xc2f\/X5ub\/fnh0\/6OFh\/+9iI\/\/tHh+\/5lgYf+nbHD\/z6iu\/\/Pw8\/\/v9vb\/6PDw\/+Xt7f\/k7e3\/3u7u\/93s7P\/d6ur\/3Orq\/9fq6v\/W6ur\/1erq\/9Lq6v\/R6ur\/0enp\/8\/s7P\/O8vP\/zPP1\/5WRlv9jQ0j\/Uzk8\/2RXWf+qp6T\/197a\/9fm5\/95cm\/\/q4iM\/8KJkv+ka3L\/ekxO\/5dmav\/RrrL\/\/Pj5\/\/r+\/\/\/5\/\/\/\/+P\/\/\/\/f\/\/\/\/y\/\/\/\/8P\/\/\/\/H\/\/\/\/w\/\/\/\/6\/\/\/\/+n\/\/\/\/m\/\/\/\/4v\/\/\/9\/\/\/\/\/f\/\/\/\/3\/\/\/\/9v9\/f\/O7e\/\/k4iM\/2A9Qv9TOjz\/a2Zj\/7Cwq\/\/Z39z\/2Obn\/3Zqav+wiY7\/05Ob\/8J5gP+jXmX\/uHl\/\/+C8wP\/+\/f3\/\/v\/\/\/\/z\/\/\/\/7\/\/\/\/+\/\/\/\/\/n\/\/\/\/2\/\/\/\/9P\/\/\/\/L+\/v\/w\/\/\/\/7\/\/\/\/+3\/\/\/\/q\/\/\/\/5P\/\/\/+P\/\/\/\/l\/\/\/\/4\/v7\/9Lk5f+UgYb\/XTxA\/1NDQf+HhX7\/wMbC\/9vm5\/\/Y6Or\/b19g\/6N9gv\/HjJT\/wn6F\/7lxeP\/Dio\/\/1rm7\/+Xk5P\/k4uL\/4eDg\/97e3v\/c3d3\/2tvb\/9fZ2f\/Z4OD\/3Ofn\/9PY1\/\/Q1NP\/ztTT\/8vS0v\/Ezs7\/xM3N\/8TMy\/\/AxMP\/rqqs\/4Nxc\/9zY2L\/joyH\/7e+uv\/Q3d3\/2ens\/9jp7P+ZlZf\/s6Wq\/8evtf\/Iq7H\/yqmu\/8y2uf\/Ry83\/1tvd\/9Ta2\/\/S2Nn\/0NbX\/87U1v\/N0tT\/y9HS\/87X2f\/S3+D\/ydDQ\/8bMzP\/FzMz\/w8rL\/7\/Gx\/+\/xcb\/v8TE\/7y+v\/+xrrH\/n52f\/6eqqf\/Ez8z\/1ePj\/9jp6\/\/Y6ez\/2Ons\/9fo6\/\/Y6Ov\/2Ojr\/9jo6\/\/Y6Ov\/2Ojr\/9jp7P\/Y6ez\/2Ons\/9jp7P\/Y6ez\/2Ons\/9jp7P\/Y6ez\/2Ons\/9jp7P\/Y6ez\/2Ons\/9jp7P\/Y6ez\/2Ons\/9jp7P\/Y6ez\/2Ons\/9jo6\/\/X6Ov\/1+jr\/9jp7P\/Y6ez\/2Ons\/9jp7P\/Y6ez\/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=");

            byte[] bytes = Convert.FromBase64String(Text);

            Image image;
            using (Stream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            image.Save("Coffee2.png",ImageFormat.Png);

            return image;
        }
        
    }
}
