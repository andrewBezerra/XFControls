using Xamarin.Forms;

namespace XFControls.Controls
{
    public class TextInput : Entry
    {
        public static readonly BindableProperty LeadingIconProperty =
     BindableProperty.Create(nameof(LeadingIcon),
     typeof(string),
     typeof(TextInput),
     null);

        public string LeadingIcon
        {
            get => (string)GetValue(LeadingIconProperty);
            set => SetValue(LeadingIconProperty, value);
        }

        public static readonly BindableProperty TrailingIconProperty =
          BindableProperty.Create(nameof(TrailingIcon),
          typeof(string),
          typeof(TextInput),
          null);

        public string TrailingIcon
        {
            get => (string)GetValue(TrailingIconProperty);
            set => SetValue(TrailingIconProperty, value);
        }

        public static readonly BindableProperty BorderRadiusProperty =
           BindableProperty.Create(nameof(BorderRadius),
           typeof(int),
           typeof(TextInput),
           0);

        public int BorderRadius
        {
            get => (int)GetValue(BorderRadiusProperty);
            set => SetValue(BorderRadiusProperty, value);
        }

        public static readonly BindableProperty CounterMaxProperty =
         BindableProperty.Create(nameof(CounterMax),
         typeof(int),
         typeof(TextInput),
         0);

        public int CounterMax
        {
            get => (int)GetValue(CounterMaxProperty);
            set => SetValue(CounterMaxProperty, value);
        }

        public static readonly BindableProperty HelpTextProperty =
          BindableProperty.Create(nameof(HelpText),
          typeof(string),
          typeof(TextInput),
          null);

        public string HelpText
        {
            get => (string)GetValue(HelpTextProperty);
            set => SetValue(HelpTextProperty, value);
        }

        public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText),
        typeof(string),
        typeof(TextInput),
        null);

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public Container ContainerType { get; set; } = Container.Filled;

        public bool IsDisposed()
        {
            return this == null;
        }

        public bool IsAlive()
        {
            if (this == null)
                return false;

            return !this.IsDisposed();
        }

        public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor),
        typeof(Color),
        typeof(TextInput),
        null);
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);

        }

    }

    public enum Container
    {
        None,
        Filled,
        Outlined
    }
}