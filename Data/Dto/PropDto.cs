namespace Data.Models;

public class PropDto
{
   public string Text { get; set; }
   public string Value{ get; set; }
   public override bool Equals(object? x)
   {
      if (x != null)
      {
         if (x is PropDto propx)
         {
            return propx.Value == Value;
         }
      }
      return false;
   } 
}