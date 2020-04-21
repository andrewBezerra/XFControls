using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFControls.Controls;
using XFControls.Droid.Renderers;
using AColor = Android.Graphics.Color;
using Color = Xamarin.Forms.Color;
using FormsAppCompat = Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(TextInput), typeof(EntryCustomRenderer))]

namespace XFControls.Droid.Renderers
{
    public class EntryCustomRenderer : FormsAppCompat.ViewRenderer<TextInput, TextInputLayout>, ITextWatcher,
        TextView.IOnEditorActionListener
    {
        private const int DefaultPadding = 40;
        private static readonly Color SemiTransparent = Color.FromHex("#80000000");

        protected AColor GetPlaceholderColor() => ColorHelper.ConvertToAndroid(Element?.PlaceholderColor, Color.LightGray);

        public EntryCustomRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override TextInputLayout CreateNativeControl()
        {
            var textInputLayout = new TextInputLayout(Context);
            var editText = new AppCompatEditText(Context)
            {
                SupportBackgroundTintList = ColorStateList.ValueOf(GetPlaceholderColor())
            };

            editText.SetTextSize(ComplexUnitType.Sp, (float)Element.FontSize);
            editText.InputType = Element.Keyboard.ToInputType();

            textInputLayout.AddView(editText);
            return textInputLayout;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TextInput> e)
        {
            base.OnElementChanged(e);

            if (!(Control is TextInputLayout))
                SetNativeControl(CreateNativeControl());

            Control.EditText.AddTextChangedListener(this);
            Control.EditText.SetOnEditorActionListener(this);
            Control.EditText.ImeOptions = ImeAction.Done;
            Control.EditText.SetElegantTextHeight(true);

            SetBorderColor();
            SetHintText();
            SetHelperText();
            SetBoxBackgroundMode();
            SetBorderRadius();
            SetCounterMax();
            SetPasswordMode();
            SetIcons();
        }

        private void SetBorderColor()
        {
            Control.BoxStrokeColor = ColorHelper.ConvertToAndroid(Element.BorderColor, Color.Gray);
        }

        private void SetIcons()
        {
            Control.EditText.SetCompoundDrawablesRelativeWithIntrinsicBounds(
                Context.TryGetDrawable(Element?.LeadingIcon), null, Context.TryGetDrawable(Element?.TrailingIcon), null);

            if (!string.IsNullOrEmpty(Element.LeadingIcon) || !string.IsNullOrEmpty(Element.TrailingIcon))
                Control.EditText.CompoundDrawablePadding = DefaultPadding;
        }
       

        private void SetPasswordMode()
        {
            if (!Element.IsPassword) return;

            //Garantir que estamos descartando objetos nao usados
            Control.EditText?.TransformationMethod?.Dispose();
            Control.EditText.TransformationMethod = new PasswordTransformationMethod();
            Control.PasswordVisibilityToggleEnabled = true;
        }

        private void SetBorderRadius()
        {
            var sidePadding = DefaultPadding;

            Control.SetBoxCornerRadii(Element.BorderRadius, Element.BorderRadius,
                GetBottomBorderRadius(), GetBottomBorderRadius());

            Control.EditText.SetPadding(sidePadding, DefaultPadding, sidePadding, DefaultPadding);
        }

        private int GetBottomBorderRadius()
        {
            return Element.ContainerType == Container.Outlined
                ? Element.BorderRadius : 0;
        }

        private void SetBoxBackgroundMode()
        {
            Control.SetBoxBackgroundMode((int)Element.ContainerType);

            if (Element.ContainerType != Container.Outlined)
                Control.BoxBackgroundColor = Color.FromHex("#eeeeee").ToAndroid();
        }

        private void SetCounterMax()
        {
            Control.CounterEnabled = Element.CounterMax > 0;
            Control.CounterMaxLength = Element.CounterMax;
        }

        private void SetHelperText()
        {
            Control.HelperTextEnabled = !string.IsNullOrEmpty(Element.HelpText);
            Control.HelperText = Element.HelpText;
        }

        private void SetHintText()
        {
            Control.Hint = Element.Placeholder;
        }

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            if (Element == null || !Control.IsAlive() || !Control.EditText.IsAlive())
                return false;

            if ((actionId != ImeAction.ImeNull) || ((actionId == ImeAction.ImeNull) && (e.KeyCode == Keycode.Enter)))
            {
                Control.ClearFocus();
                ((IEntryController)Element).SendCompleted();
            }

            return true;
        }

        #region ITextWatcher

        public virtual void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if ((!Element.IsAlive() && (s.Length() == 0)) || !Control.IsAlive() || !Control.EditText.IsAlive())
                return;

            ((IElementController)Element)?.SetValueFromRenderer(Entry.TextProperty, s.ToString());
        }

        public void AfterTextChanged(IEditable s)
        {
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        #endregion ITextWatcher
    }

    internal static class ColorHelper
    {
        public static global::Android.Graphics.Color ConvertToAndroid(Color? colorToConvert, Color defaultValue)
        {
            return (colorToConvert ?? defaultValue).ToAndroid();
        }
    }

    internal static class ContextExtensions
    {
        public static Drawable TryGetDrawable(this Context context, string drawableName)
        {
            return !string.IsNullOrWhiteSpace(drawableName)
                    ? context.GetDrawable(drawableName) : null;
        }
    }

    internal static class VisualElementExtensions
    {
        public static bool IsAlive<T>(this VisualElementRenderer<T> renderer) where T : VisualElement
        {
            if (renderer == null)
                return false;

            var element = renderer.Element;
            if (element is Entry entryElement)
            {
                if (string.IsNullOrWhiteSpace(entryElement.Text))
                    return false;
            }

            if (element is Label labelElement)
            {
                if (string.IsNullOrWhiteSpace(labelElement.Text))
                    return false;
            }

            return element != null;
        }
    }

    internal static class JavaObjectExtensions
    {
        public static bool IsDisposed(this Java.Lang.Object obj)
        {
            return obj.Handle == IntPtr.Zero;
        }

        public static bool IsAlive(this Java.Lang.Object obj)
        {
            if (obj == null)
                return false;

            return !obj.IsDisposed();
        }
    }
}