using System;
using System.Windows;
using helpinterop1;

namespace JaStDev.ControlFramework.Controls
{
   /// <summary>
   /// Generic class to integrate help into a wpf project.
   /// </summary>
   /// <remarks>
   /// <para>
   /// This class provides an easy way to include context sensitive help in wpf projects.
   /// </para>
   /// <para>
   /// The basic steps to integrate help are:
   /// <list type="number">
   ///   <item>
   ///      <description>
   ///         Add a Help field to the project (usually the app or main window class).
   ///      </description>
   ///   </item>
   ///   <item>
   ///      <description>
   ///         Add a CommandBinding to the control handling help (usually the main window) for the Help command and provide the CanExecute and Executed events.
   ///      </description>
   ///   </item>
   ///   <item>
   ///      <description>
   ///         Implement the commandBinding's CanExecute event by checking if there is a control that has keyboard focus.
   ///      </description>
   ///   </item>
   ///   <item>
   ///      <description>
   ///         Implement the commandBinding's Executed event by calling <see cref="Help.ShowHelpFor"/> for the control that has keyboard focus.
   ///      </description>
   ///   </item>
   /// </list>
   /// And that's it.  
   /// </para>
   /// <para>
   /// To finish it of, there are also functions to show the help file or parts of it such as <see cref="Help.ShowHelp"/>, 
   /// <see cref="Help.ShowHelpIndex"/>, <see cref="Help.ShowHelpSearch"/>, <see cref="Help.ShowHelpContents"/> and  <see cref="Help.ShowHelpTopic"/>.
   /// </para>
   /// </remarks>
   public class Help: DependencyObject
   {

      #region fields

      string fHelpFile = null;

      #endregion

      #region prop

      /// <summary>
      /// Identifies the Topic attached property.
      /// </summary>
      public static readonly DependencyProperty TopicProperty =
            DependencyProperty.RegisterAttached("Topic", typeof(string), typeof(Help), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

      /// <summary>
      /// Sets the context sensitive help topic for the specified element.
      /// </summary>
      /// <remarks>
      /// Use this function (or the related attached property) to assign a topic to a UIElement.  The topic should be the name of the html file 
      /// containging the context sesitive help of the control.
      /// </remarks>
      /// <example>
      /// <code lang="xml">
      /// <![CDATA[
      /// <Button help:Help.Topic="Button.html">
      ///    Button with help
      /// </Button>
      /// ]]>
      /// </code>
      /// </example>
      public static void SetTopic(UIElement element, string value)
      {
         element.SetValue(TopicProperty, value);
      }


      /// <summary>
      /// Gets the context sensitive help topic for the specified element.
      /// </summary>
      /// <remarks>
      /// See <see cref="Help.SetTopic"/> for more info.
      /// </remarks>
      public static string GetTopic(UIElement element)
      {
         return (string)element.GetValue(TopicProperty);
      }

      /// <summary>
      /// Identifies the HelpFile attached property.
      /// </summary>
      public static readonly DependencyProperty HelpFileProperty =
            DependencyProperty.RegisterAttached("HelpFile", typeof(string), typeof(Help), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

      /// <summary>
      /// Sets the file to retrieve the context sensitive help topic from for the specified element.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Use this function (or the related attached property) to assign a different file then <see cref="Help.DefaultHelpFile"/> to retrieve the topic from.
      /// </para>
      /// <para>
      /// This attached property is especially interesting for projects that can be extended.  This allows the extender of the wpf file to provide 
      /// context sensitive help through the application's standard help system without having to modify the original help file.  
      /// </para>
      /// </remarks>
      /// <example>
      /// <code lang="xml">
      /// <![CDATA[
      /// <Button help:Help.Topic="Button.html"
      ///         help:Help.HelpFile="Help2.chm">
      ///    button with help from other file
      /// </Button>
      /// ]]>
      /// </code>
      /// </example>
      public static void SetHelpFile(UIElement element, string value)
      {
         element.SetValue(HelpFileProperty, value);
      }


      /// <summary>
      /// Gets the file to retrieve the context sensitive help topic for the specified element from.
      /// </summary>
      /// <remarks>
      /// See <see cref="Help.SetHelpFile"/> for more info.
      /// </remarks>
      public static string GetHelpFile(UIElement element)
      {
         return (string)element.GetValue(HelpFileProperty);
      }

      /// <summary>
      /// Gets/sets the global help file to use.
      /// </summary>
      /// <remarks>
      /// When a control doesn't have <see cref="Help.SetHelpFile"/> attached, the <see cref="Help.ShowHelpFor"/> uses this
      /// file to search the topic in.  This file is also shown when <see cref="Help.ShowHelp"/> or related functions is called.
      /// </remarks>
      public string DefaultHelpFile
      {
         get { return fHelpFile; }
         set { fHelpFile = value; }
      }

      #endregion

      #region functions


      /// <summary>
      /// Displays the help file.
      /// </summary>
      /// <remarks>
      /// <para>
      /// The file being displayed always comes from the <see cref="Help.DefaultHelpFile"/>
      /// </para>
      /// <para>
      /// When the help file was not yet opened, the table of contents is shown, otherwise, the already opened help file is brought to the foreground.
      /// </para>
      /// <para>
      /// Related functions are:
      /// <list type="bullet">
      ///   <item>
      ///      <description>
      ///         <see cref="Help.ShowHelpTopic"/>
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <description>
      ///         <see cref="Help.ShowHelpContents"/>
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <description>
      ///         <see cref="Help.ShowHelpSearch"/>
      ///      </description>
      ///   </item>
      ///   <item>
      ///      <description>
      ///         <see cref="Help.ShowHelpIndex"/>
      ///      </description>
      ///   </item>
      /// </list>
      /// </para>
      /// </remarks>
      public void ShowHelp()
      {
         if (DefaultHelpFile != null)
         {
            HH1Interop.HtmlHelp_DisplayTopic(0, DefaultHelpFile);
         }
      }

      /// <summary>
      /// Displays the context sensitive help for the specified object.
      /// </summary>
      /// <remarks>
      /// If the control has a <see cref="Help.SetHelpFile"/> attached, this file will be used, otherwise, the 
      /// <see cref="Help.DefaultHelpFile"/> is used to search the topic in.  if there is no topic, or no file specified (no default 
      /// or specific file), no help is shown.
      /// </remarks>
      /// <param name="item">The object to display help for.</param>
      public void ShowHelpFor(UIElement item)
      {
         string iTopicId = GetTopic(item);
         string iFile = GetFile(item);

         if (iTopicId != null && iFile != null)
         {
            HH1Interop.HtmlHelp_DisplayTopic(0, iFile + "::/" + iTopicId);
         }
      }

      /// <summary>
      /// Displays a specific help topic from the default file.
      /// </summary>
      /// <remarks>
      /// the Topic id is the name of the html file.
      /// </remarks>
      /// <example>
      /// <code>
      /// Help.ShowHelpTopic("gettingstarted.html");
      /// </code>
      /// </example>
      /// <param name="topicId">The topic to display</param>
      /// <seealso cref="Help.ShowHelp"/>
      public void ShowHelpTopic(string topicId)
      {
         if (topicId != null && DefaultHelpFile != null)
         {
            HH1Interop.HtmlHelp_DisplayTopic(0, DefaultHelpFile + "::/" + topicId);
         }
      }

      /// <summary>
      /// Displays the content page of the default help file.
      /// </summary>
      /// <remarks>
      /// This function will always show the content pages, even if the help file was already opened while <see cref="Help.ShowHelp"/> will
      /// simply bring the help to foreground if it was already open.
      /// </remarks>
      /// <seealso cref="Help.ShowHelp"/>
      public void ShowHelpContents()
      {
         if (DefaultHelpFile != null)
         {
            HH1Interop.HtmlHelp_DisplayTOC(0, DefaultHelpFile);
         }
      }

      /// <summary>
      /// Displays the search page of the default help file.
      /// </summary>
      /// <seealso cref="Help.ShowHelp"/>
      public void ShowHelpSearch()
      {
         if (DefaultHelpFile != null)
         {
            HH1Interop.HH_FTS_QUERY query = new HH1Interop.HH_FTS_QUERY();
            int res = HH1Interop.HtmlHelp_DisplaySearch(0, DefaultHelpFile, ref query);
         }
      }


      /// <summary>
      /// Displays the index page of the default help file.
      /// </summary>
      /// <param name="start">A possible default value for the index search box, can be null.</param>
      public void ShowHelpIndex(string start)
      {
         if (DefaultHelpFile != null)
         {
            HH1Interop.HtmlHelp_DisplayIndex(0, DefaultHelpFile, start);
         }
      }

      /// <summary>
      /// Gets the helpfile to use for the specified object.
      /// </summary>
      /// <remarks>
      /// This can either be the context sensitive file, if assigned, or it is the global file.
      /// </remarks>
      /// <param name="item">The object to get the helpfile for.</param>
      /// <returns>The file name or null.</returns>
      private string GetFile(UIElement item)
      {
         string iRes = GetHelpFile(item);
         if (iRes == null)
         {
            iRes = DefaultHelpFile;
         }
         return iRes;
      }


      #endregion

   }
}
