using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCustomControl
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfCustomControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfCustomControl;assembly=WpfCustomControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:KeyCombination/>
    ///
    /// </summary>
    public class KeyCombination : Control
    {
        static KeyCombination()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyCombination), new FrameworkPropertyMetadata(typeof(KeyCombination)));
        }

        public string KeyShortcut { get; set; }



        public event EventHandler StartEditEvent;

        public event EventHandler<string> EndEditEvent;

        public List<Key> KeysList { get; set; } = new List<Key>();

        private TextBox TextBoxKeyShortcut { get; set; }

        private List<Key> AcceptKeys { get; set; } = new List<Key> { Key.LeftAlt,Key.RightAlt,Key.LeftCtrl,Key.RightCtrl,Key.LeftShift,Key.RightShift };
       

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TextBoxKeyShortcut = GetTemplateChild("textBoxKeyShortcut") as TextBox;
            TextBoxKeyShortcut.LostFocus += TextBoxKeyShortcut_LostFocus;
            TextBoxKeyShortcut.GotFocus += TextBoxKeyShortcut_GotFocus;
            TextBoxKeyShortcut.KeyDown += TextBoxKeyShortcut_KeyDown;
        }

        private void TextBoxKeyShortcut_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z ||
                AcceptKeys.Contains(e.Key))
            {
                if (HasKeyDuplicate(e.Key) == false)
                { 
                    KeysList.Add(e.Key);
                    if (!string.IsNullOrEmpty(TextBoxKeyShortcut.Text))
                    {
                        TextBoxKeyShortcut.Text += " + ";
                    }
                    TextBoxKeyShortcut.Text += e.Key.ToString();
                }
            }

            e.Handled = true;
            
        }

        private bool HasKeyDuplicate(Key key)
        {
            if ((key == Key.LeftShift || key == Key.RightShift) && HasKeyShift() ||
                    (key == Key.LeftAlt || key == Key.RightAlt) && HasKeyAlt() ||
                (key == Key.LeftCtrl || key == Key.RightCtrl) && HasKeyControl() )
                {
                return true;
            }

            if (KeysList.Exists(k => k <= Key.Z))
            {
                return true;
            }

            return false;
        }

        private bool HasKeyAlt()
        {
            return KeysList.Contains(Key.LeftAlt) || KeysList.Contains(Key.RightAlt);
        }

        private bool HasKeyControl()
        {
            return KeysList.Contains(Key.LeftCtrl) || KeysList.Contains(Key.RightCtrl);
        }

        private bool HasKeyShift()
        {
            return KeysList.Contains(Key.LeftShift) || KeysList.Contains(Key.RightShift);
        }

        private void TextBoxKeyShortcut_GotFocus(object sender, RoutedEventArgs e)
        {
            StartEditEvent?.Invoke(this, EventArgs.Empty);
        }

        private void TextBoxKeyShortcut_LostFocus(object sender, RoutedEventArgs e)
        {
            EndEditEvent?.Invoke(this, KeyShortcut);
        }
    }
}
