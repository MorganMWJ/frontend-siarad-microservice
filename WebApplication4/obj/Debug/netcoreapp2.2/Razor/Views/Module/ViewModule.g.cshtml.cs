#pragma checksum "M:\FrontEnd\WebApplication4\Views\Module\ViewModule.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c4a48e7999776e84a2d6fc8f241a43d50b1c4882"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Module_ViewModule), @"mvc.1.0.view", @"/Views/Module/ViewModule.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Module/ViewModule.cshtml", typeof(AspNetCore.Views_Module_ViewModule))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "M:\FrontEnd\WebApplication4\Views\_ViewImports.cshtml"
using WebApplication4;

#line default
#line hidden
#line 2 "M:\FrontEnd\WebApplication4\Views\_ViewImports.cshtml"
using WebApplication4.Models;

#line default
#line hidden
#line 3 "M:\FrontEnd\WebApplication4\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c4a48e7999776e84a2d6fc8f241a43d50b1c4882", @"/Views/Module/ViewModule.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f26ec57f2012adb4f6ede13aef122d1f4fa373d0", @"/Views/_ViewImports.cshtml")]
    public class Views_Module_ViewModule : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ModuleViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(24, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "M:\FrontEnd\WebApplication4\Views\Module\ViewModule.cshtml"
  
    ViewBag.Title = "Module View";

#line default
#line hidden
#line 6 "M:\FrontEnd\WebApplication4\Views\Module\ViewModule.cshtml"
 if (ViewBag.Error == "Error")
{

#line default
#line hidden
            BeginContext(104, 92, true);
            WriteLiteral("    <div class=\"text-center\">\r\n        <h1>Error connecting to server</h1>\r\n        </div>\r\n");
            EndContext();
#line 11 "M:\FrontEnd\WebApplication4\Views\Module\ViewModule.cshtml"
        }
        else
        {

#line default
#line hidden
            BeginContext(232, 108, true);
            WriteLiteral("        <div class=\"text-center\">\r\n            <h1>Module View</h1>\r\n            <p>This is in Module View: ");
            EndContext();
            BeginContext(341, 10, false);
#line 16 "M:\FrontEnd\WebApplication4\Views\Module\ViewModule.cshtml"
                                  Write(Model.Code);

#line default
#line hidden
            EndContext();
            BeginContext(351, 5, true);
            WriteLiteral(" --- ");
            EndContext();
            BeginContext(357, 11, false);
#line 16 "M:\FrontEnd\WebApplication4\Views\Module\ViewModule.cshtml"
                                                  Write(Model.Title);

#line default
#line hidden
            EndContext();
            BeginContext(368, 22, true);
            WriteLiteral("</p>\r\n        </div>\r\n");
            EndContext();
#line 18 "M:\FrontEnd\WebApplication4\Views\Module\ViewModule.cshtml"


        }

#line default
#line hidden
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ModuleViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591