using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace JaStDev.ControlFramework.Utility
{
   /// <summary>
   /// A class that inserts a string in the parameter passed to the converter if the value that needs converting is boolean 'True' value.
   /// </summary>
   /// <remarks>
   /// <para>
   /// There are 2 important properties: <see cref="InsertTextConverter.Text"/> determins which string is inserted, and 
   /// <see cref="InsertTextConverter.Split"/> determins where the parameter value is split.
   /// </para>
   /// This class can be used to display a different image in a button depending on wether it is enabled or not. This allows you to
   /// show disabled images in a toolbar. As an example: if the Text prop = "Enabled", the Split = ".png" and the parameter value
   /// = "\images\cut.png", the result would be "\images\cutEnabled.png"
   /// </remarks>
   public class InsertTextConverter: IValueConverter
   {
      /// <summary>
      /// Gets/sets the string to append to the parameter during convertion.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Determins where the <see cref="InsertTextConverter.Text"/> property is inserted in the Parameter value during convertion.
      /// </summary>
      /// <remarks>
      /// If the split text isn't found in the parameter value, the Text value is appended to it.
      /// </remarks>
      public string Split { get; set; }

      #region IValueConverter Members

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         string iParam = parameter as string;
         if (iParam != null && value is bool)
         {
            if ((bool)value == true)
            {
               string[] iSplit = iParam.Split(new string[] { Split }, StringSplitOptions.None);
               StringBuilder iRes = new StringBuilder();
               if (iSplit.Length > 1)
               {
                  for (int i = 0; i < iSplit.Length - 1; i++)                                      //can't add at the end.
                  {
                     iRes.Append(iSplit[i]);
                     iRes.Append(Text);
                     iRes.Append(Split);
                  }
                  return iRes.ToString();
               }
               else
                  return iParam + Text;
            }
            else
               return iParam;
         }
         return parameter;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
