using System.Text.RegularExpressions;
using GitTool;
using NUnit.Framework;

namespace Tests.GitTool
{
    [TestFixture]
    public class GitToolTests
    {
        private const string GitTestDir = "D:\\MyRepository\\GitHub\\JCore\\GitTool\\bin\\Debug\\MyList";
        private static readonly Regex DateRegex = new Regex("new Date\\(\\d*\\.?\\d*\\)");
        [Test]
        public static void Temp()
        {
            string a = "1";
            string b = null;

            var c = b ?? "";
            var d = a ?? "";
            var str = "$axure.loadCurrentPage(\r\n(function() {\r\n    var _ = function() { var r={},a=arguments; for(var i=0; i<a.length; i+=2) r[a[i]]=a[i+1]; return r; }\r\n    var _creator = function() { return _(b,c,d,e,f,g,h,g,i,_(j,k),l,[m],n,_(o,p,q,r,s,t,u,_(),v,_(w,x,y,z,A,_(B,C,D,E),F,null,G,z,H,z,I,J,K,null,L,M,N,O,P,Q,R,M),S,_(),T,_(),U,_(V,[_(W,X,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bf,_(bg,bh,bi,bj),w,bk,bl,_(bm,bn,bo,bp),bq,br,bs,_(B,C,D,bt)),S,_(),bu,_(),bv,g),_(W,bw,Y,j,Z,bx,q,by,bc,by,bd,be,v,_(w,bz,bf,_(bg,bA,bi,bA),bl,_(bm,bB,bo,bC)),S,_(),bu,_(),bD,_(bE,bF)),_(W,bG,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bH,_(B,C,D,E,bI,bJ),bf,_(bg,bK,bi,bL),w,bM,bl,_(bm,bN,bo,bO),bP,bQ,A,_(B,C,D,bR),bS,bT,bq,bU),S,_(),bu,_(),bv,g),_(W,bV,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bf,_(bg,bW,bi,bX),w,bk,bl,_(bm,bY,bo,bZ),bs,_(B,C,D,E),A,_(B,C,D,ca)),S,_(),bu,_(),bv,g),_(W,cb,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bH,_(B,C,D,cc,bI,bJ),bf,_(bg,bn,bi,cd),w,bM,bl,_(bm,bO,bo,bZ),bP,ce),S,_(),bu,_(),bv,g),_(W,cf,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bH,_(B,C,D,cc,bI,bJ),bf,_(bg,cg,bi,cd),w,bM,bl,_(bm,ch,bo,bA),bP,ce),S,_(),bu,_(),bv,g),_(W,ci,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bf,_(bg,bh,bi,bj),w,bk,bl,_(bm,bn,bo,cj),bq,br,bs,_(B,C,D,bt)),S,_(),bu,_(),bv,g),_(W,ck,Y,j,Z,bx,q,by,bc,by,bd,be,v,_(w,bz,bf,_(bg,cl,bi,cl),bl,_(bm,bN,bo,cj)),S,_(),bu,_(),bD,_(bE,cm)),_(W,cn,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bf,_(bg,co,bi,bX),w,bk,bl,_(bm,cp,bo,cq),bs,_(B,C,D,E),A,_(B,C,D,ca)),S,_(),bu,_(),bv,g),_(W,cr,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bH,_(B,C,D,cs,bI,bJ),bf,_(bg,ct,bi,cu),w,bM,bl,_(bm,cv,bo,bh),bP,cw),S,_(),bu,_(),bv,g),_(W,cx,Y,j,Z,ba,q,bb,bc,bb,bd,be,v,_(bH,_(B,C,D,cs,bI,bJ),bf,_(bg,bn,bi,cy),w,bM,bl,_(bm,cz,bo,cA),bP,cB),S,_(),bu,_(),bv,g)])),cC,_(),cD,_(cE,_(cF,cG),cH,_(cF,cI),cJ,_(cF,cK),cL,_(cF,cM),cN,_(cF,cO),cP,_(cF,cQ),cR,_(cF,cS),cT,_(cF,cU),cV,_(cF,cW),cX,_(cF,cY),cZ,_(cF,da)));}; \r\nvar b=\"url\",c=\"banner___.html\",d=\"generationDate\",e=new Date(1582185472813.15),f=\"isCanvasEnabled\",g=false,h=\"isAdaptiveEnabled\",i=\"sketchKeys\",j=\"\",k=\"s0\",l=\"variables\",m=\"OnLoadVariable\",n=\"page\",o=\"packageId\",p=\"e984962f8976413db60ce4ec11386405\",q=\"type\",r=\"Axure:Page\",s=\"name\",t=\"Banner图需求\",u=\"notes\",v=\"style\",w=\"baseStyle\",x=\"627587b6038d43cca051c114ac41ad32\",y=\"pageAlignment\",z=\"near\",A=\"fill\",B=\"fillType\",C=\"solid\",D=\"color\",E=0xFFFFFFFF,F=\"image\",G=\"imageHorizontalAlignment\",H=\"imageVerticalAlignment\",I=\"imageRepeat\",J=\"auto\",K=\"favicon\",L=\"sketchFactor\",M=\"0\",N=\"colorStyle\",O=\"appliedColor\",P=\"fontName\",Q=\"Applied Font\",R=\"borderWidth\",S=\"adaptiveStyles\",T=\"interactionMap\",U=\"diagram\",V=\"objects\",W=\"id\",X=\"f4fdbc0b4b824a7aa09fa45868efc5fd\",Y=\"label\",Z=\"friendlyType\",ba=\"矩形\",bb=\"vectorShape\",bc=\"styleType\",bd=\"visible\",be=true,bf=\"size\",bg=\"width\",bh=298,bi=\"height\",bj=132,bk=\"4b7bfc596114427989e10bb0b557d0ce\",bl=\"location\",bm=\"x\",bn=65,bo=\"y\",bp=54,bq=\"cornerRadius\",br=\"5\",bs=\"borderFill\",bt=0xFFAEAEAE,bu=\"imageOverrides\",bv=\"generateCompound\",bw=\"937904e66e454b31b0b2b316db256b66\",bx=\"SVG\",by=\"imageBox\",bz=\"75a91ee5b9d042cfa01b8d565fe289c0\",bA=118,bB=219,bC=64,bD=\"images\",bE=\"normal~\",bF=\"images/banner___/u433.svg\",bG=\"2c04381fae6b4e66a54c8068ca3d6431\",bH=\"foreGroundFill\",bI=\"opacity\",bJ=1,bK=69,bL=48,bM=\"2285372321d148ec80932747449c36c9\",bN=244,bO=90,bP=\"fontSize\",bQ=\"18px\",bR=0xFFF2CB51,bS=\"verticalAlignment\",bT=\"middle\",bU=\"20\",bV=\"dd72d3688600425c8051b323ad03afb1\",bW=146,bX=97,bY=213,bZ=75,ca=0xB2FFFFFF,cb=\"922714640fbb4580af4716ef86ed3e04\",cc=0xFFFFCC00,cd=33,ce=\"28px\",cf=\"a51c589bfac649deb0916308a440bd7b\",cg=137,ch=122,ci=\"2d9c90c567a24a12b5c2af1d6c607c02\",cj=257,ck=\"600d4454603d477da7cd7cdc76fbc2f9\",cl=119,cm=\"images/banner___/u439.svg\",cn=\"aff2b3a8f0b147d0bc8f790c1c504cb9\",co=103,cp=256,cq=268,cr=\"f773e8a30eab40a58da5b96b2b3838a9\",cs=0xFFFC355D,ct=173,cu=32,cv=79,cw=\"26px\",cx=\"9030ca36f19c4d7eb54a0d3c32e14053\",cy=19,cz=80,cA=337,cB=\"16px\",cC=\"masters\",cD=\"objectPaths\",cE=\"f4fdbc0b4b824a7aa09fa45868efc5fd\",cF=\"scriptId\",cG=\"u432\",cH=\"937904e66e454b31b0b2b316db256b66\",cI=\"u433\",cJ=\"2c04381fae6b4e66a54c8068ca3d6431\",cK=\"u434\",cL=\"dd72d3688600425c8051b323ad03afb1\",cM=\"u435\",cN=\"922714640fbb4580af4716ef86ed3e04\",cO=\"u436\",cP=\"a51c589bfac649deb0916308a440bd7b\",cQ=\"u437\",cR=\"2d9c90c567a24a12b5c2af1d6c607c02\",cS=\"u438\",cT=\"600d4454603d477da7cd7cdc76fbc2f9\",cU=\"u439\",cV=\"aff2b3a8f0b147d0bc8f790c1c504cb9\",cW=\"u440\",cX=\"f773e8a30eab40a58da5b96b2b3838a9\",cY=\"u441\",cZ=\"9030ca36f19c4d7eb54a0d3c32e14053\",da=\"u442\";\r\nreturn _creator();\r\n})());";
            str = DateRegex.Replace(str, "new Date()");
        }

        [Test]
        public static void Init()
        {
            GitUtils.Init();
        }

        [Test]
        public static void SetQuotePath()
        {
            var res = GitUtils.SetQuotePath();
        }

        [Test]
        public static void GitClone()
        {
            var remote = "http://git.server.tongbu.com/youxiaojun/MyList.git";
            var account = "664105020@qq.com";
            var password = "a5625845";
            var res = GitUtils.GitClone(remote, account, password);
        }

        [Test]
        public static void GitPull()
        {
            var dir = "D:\\MyRepository\\GitHub\\JCore\\GitTool\\bin\\Debug";
            var res = GitUtils.GitPull(dir);
            var res1 = GitUtils.GitPull(GitTestDir);
        }

        [Test]
        public static void GitPush()
        {
            GitUtils.GitPush(GitTestDir, "data.js push test");
        }
        [Test]
        public static void GitCheckout()
        {
            var res = GitUtils.GitCheckout(GitTestDir, "README.md");
        }

        [Test]
        public static void GitStash()
        {
            var res= GitUtils.GitStash(GitTestDir);
            res= GitUtils.GitStash(GitTestDir,"pop");
        }

        [Test]
        public static void GitCommit()
        {
            GitUtils.GitCommit(GitTestDir, "更新");
        }

        [Test]
        public static void GitReset()
        {
            var fileName = "README.md";
            var sha1 = GitUtils.GetPreSha1(GitTestDir);
            var res = GitUtils.GitReset(GitTestDir, sha1, fileName);
        }

        [Test]
        public static void GitStatus()
        {
            var res = GitUtils.GitStatus(GitTestDir);
        }

        [Test]
        public static void GitAdd()
        {
            var res = GitUtils.GitAdd(GitTestDir);
        }

        [Test]
        public static void GetSpecialSha1File()
        {
            var fileName = "README.md";
            var sha1 = GitUtils.GetPreSha1(GitTestDir);
            var res = GitUtils.GetSpecialSha1File(GitTestDir, sha1, fileName);
        }

        [Test]
        public static void GetPreSHA1()
        {
            //var message2 = GitUtils.GetPreSha1("D:\\MyRepository");
            var dir = "D:\\MyRepository\\GitHub\\JCore\\GitTool\\bin\\Debug\\MyList";
            var message = GitUtils.GetPreSha1(GitTestDir);
        }
    }
}
