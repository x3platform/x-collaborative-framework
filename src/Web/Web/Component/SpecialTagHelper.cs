namespace X3Platform.Web.Component
{
    using System.Text.RegularExpressions;

    public class SpecialTagHelper
    {
        public static string Decode(string text)
        {
            text = Regex.Replace(text, @"{youku:(.*?)}",
                "<div class=\"text-center\" ><embed src=\"http://player.youku.com/player.php/sid/$1/v.swf\" quality=\"high\" width=\"480\" height=\"400\" align=\"middle\" allowScriptAccess=\"allways\" mode=\"transparent\" type=\"application/x-shockwave-flash\"></embed></div>");
            
            text = Regex.Replace(text, @"{tudou:(.*?)}",
                "<div class=\"text-center\" ><embed src=\"http://www.tudou.com/v/$1\" quality=\"high\" width=\"480\" height=\"400\" align=\"middle\" allowScriptAccess=\"allways\" mode=\"transparent\" type=\"application/x-shockwave-flash\"></embed></div>");
            
            text = Regex.Replace(text, @"{ku6:(.*?)}",
                "<div class=\"text-center\" ><embed src=\"http://player.ku6.com/refer/$1/v.swf\" quality=\"high\" width=\"480\" height=\"400\" align=\"middle\" allowScriptAccess=\"allways\" mode=\"transparent\" type=\"application/x-shockwave-flash\"></embed></div>");
            
            return text;
        }
    }
}
