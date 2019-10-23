using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using AColor = Android.Graphics.Color;
using Color = Xamarin.Forms.Color;
using FormsAppCompat = Xamarin.Forms.Platform.Android.AppCompat;
using Android.Support.Design.Widget;
using Xamarin.Forms.Platform.Android;
using Android.Support.V7.Widget;
using Android.Content.Res;
using Android.Util;
using Android.Text.Method;
using Java.Lang;
using Android.Text;
using Android.Views.InputMethods;
using XFControls.Controls;
using XFControls.Droid.Renderers;

[assembly: ExportRenderer(typeof(TextInput), typeof(EntryCustomRenderer))]

namespace XFControls.Droid.Renderers
{
    public class EntryCustomRenderer : FormsAppCompat.ViewRenderer<TextInput, TextInputLayout>, ITextWatcher,
        TextView.IOnEditorActionListener
    {
        public EntryCustomRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected AColor GetPlaceholderColor() => Element?.PlaceholderColor.ToAndroid(Color.FromHex("#80000000")) ?? Color.FromHex("#80000000").ToAndroid();
        protected override TextInputLayout CreateNativeControl()
        {
            ////var layout = (TextInputLayout)LayoutInflater
            ////    .From(Context)
            ////    .Inflate(Resource.Layout.TextInputLayou, null);

            //return new TextInputLayout(Context);
            var textInputLayout = new TextInputLayout(Context);
            var editText = new AppCompatEditText(Context)
            {
                SupportBackgroundTintList = ColorStateList.ValueOf(GetPlaceholderColor())
            };
            //Drawable TrailingIcon = 
            //editText.SetCompoundDrawablesWithIntrinsicBounds()
            editText.SetTextSize(ComplexUnitType.Sp, (float)Element.FontSize);

            editText.InputType = Element.Keyboard.ToInputType();
            textInputLayout.AddView(editText);
            return textInputLayout;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<TextInput> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var ctrl = CreateNativeControl();
                SetNativeControl(ctrl);
            }

            Control.EditText.AddTextChangedListener(this);
            Control.EditText.SetOnEditorActionListener(this);
            Control.EditText.ImeOptions = ImeAction.Done;
            Control.EditText.SetElegantTextHeight(true);
            //Control.EditText.SetRawInputType(InputTypes.TextVariationEmailAddress);
            //Control.EditText.SetCompoundDrawablesRelativeWithIntrinsicBounds(Context.GetDrawable(Resource.Drawable.email16), null, null, null);
            //Control.EditText.SetBackgroundColor(AColor.Gainsboro);
            SetHintText();
            SetHelperText();
            SetBoxBackgroundMode();
            SetBorderRadius();
            SetCounterMax();
            SetPasswordMode();
            SetIcons();
            //await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7);

        }

        public virtual void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (Element == null || (string.IsNullOrWhiteSpace(Element.Text) && (s.Length() == 0)) || Control == null || Control.Handle == IntPtr.Zero || Control.EditText == null || Control.EditText.Handle == IntPtr.Zero)
                return;

            ((IElementController)Element)?.SetValueFromRenderer(Entry.TextProperty, s.ToString());
        }
        private void SetIcons()
        {

            Control.EditText.SetCompoundDrawablesRelativeWithIntrinsicBounds(
                Element.LeadingIcon != null ?
                Context.GetDrawable(Element.LeadingIcon) : null,
                null,
                Element.TrailingIcon != null ?
                Context.GetDrawable(Element.TrailingIcon) : null,
                null);

            if (!string.IsNullOrEmpty(Element.LeadingIcon) || !string.IsNullOrEmpty(Element.TrailingIcon))
                Control.EditText.CompoundDrawablePadding = 20;

        }
        private void SetPasswordMode()
        {

            if (Element.IsPassword)
            {
                Control.EditText.TransformationMethod = new PasswordTransformationMethod();
                Control.PasswordVisibilityToggleEnabled = true;

            }

        }
        private void SetBorderRadius()
        {

            int paddingleft = 20;
            int paddingright = 20;
            if (Element.BorderRadius >= 40)
                paddingleft = paddingright = 50;

            if (Element.ContainerType == Container.Outlined)
                Control.SetBoxCornerRadii(Element.BorderRadius, Element.BorderRadius, Element.BorderRadius, Element.BorderRadius);
            else
                Control.SetBoxCornerRadii(Element.BorderRadius, Element.BorderRadius, 0, 0);

            Control.EditText.SetPadding(paddingleft, 20, paddingright, 20);

        }
        private void SetBoxBackgroundMode()
        {

            Control.SetBoxBackgroundMode((int)Element.ContainerType);
            if (Element.ContainerType != Container.Outlined)
                Control.BoxBackgroundColor = Color.FromHex("#eeeeee").ToAndroid();
            //Control.EditText.SetBackgroundColor(AColor.Transparent);

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

        public void AfterTextChanged(IEditable s)
        {

        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {

        }

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            if (Element == null || Control == null || Control.Handle == IntPtr.Zero || Control.EditText == null || Control.EditText.Handle == IntPtr.Zero)
                return false;

            if ((actionId != ImeAction.ImeNull) || ((actionId == ImeAction.ImeNull) && (e.KeyCode == Keycode.Enter)))
            {
                Control.ClearFocus();
                //HideKeyboard();
                ((IEntryController)Element).SendCompleted();
            }

            return true;
        }
    }

}