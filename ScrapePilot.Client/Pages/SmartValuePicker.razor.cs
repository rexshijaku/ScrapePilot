using Microsoft.AspNetCore.Components;
using System.Reflection;
using ScrapePilot.Attributes;
using ScrapePilot.Client.Helpers;

namespace ScrapePilot.Client.Pages
{
    public partial class SmartValuePicker : ComponentBase
    {
        #region When Dialog Is Open
        public ModalDialog? ModalDialog { get; set; }

        [Parameter]
        public Action? StateAndResultJSONReload { get; set; }

        private const string STORE_KEY = "store_key";
        private const string DEPENDENT_FUNCTION = "dependent_function";
        private const string INDEPENDENT_FUNCTION = "independent_function";

        private string SelectedType = string.Empty;

        private string SelectedStoreKey { get; set; } = string.Empty;
        private string SelectedDependentFunction { get; set; } = string.Empty;
        private string SelectedInDependentFunction { get; set; } = string.Empty;

        private List<string> StoreKeys { get; set; } = new List<string>();
        private Dictionary<string, string> DependentFunctions { get; set; } = new Dictionary<string, string>();
        private Dictionary<string, string> InDependentFunctions { get; set; } = new Dictionary<string, string>();

        private void OnTypeDropdownChange(ChangeEventArgs args)
        {
            StoreKeys = Utility.GetTheStore();
            DependentFunctions = ReflectionHelper.GetListSource(typeof(ScrapePilot.Constants.DependentFunctions));
            InDependentFunctions = ReflectionHelper.GetListSource(typeof(ScrapePilot.Constants.IndependentFunctions));

            // Set Defaults

            SelectedStoreKey = string.Empty;

            if (DependentFunctions.Count > 0)
            {
                SelectedDependentFunction = DependentFunctions.First().Key;
            }

            if (InDependentFunctions.Count > 0)
            {
                SelectedInDependentFunction = InDependentFunctions.First().Key;
            }

            SelectedType = args.Value.ToString();
        }

        private void OnValueDropdownChanged(ChangeEventArgs args, string opt)
        {
            if (opt == STORE_KEY)
            {
                SelectedStoreKey = args.Value.ToString();
            }
            else if (opt == DEPENDENT_FUNCTION)
            {
                SelectedDependentFunction = args.Value.ToString();
            }
            else if (opt == INDEPENDENT_FUNCTION)
            {
                SelectedInDependentFunction = args.Value.ToString();
            }
        }

        public async Task OnSaved()
        {
            // ref set vals
            ModalDialog?.Close();
            this.SetTheValue(GetValue());
            this.ResetFunctionFields();
            await Task.CompletedTask;
        }

        public void OnClose()
        {
            this.ResetFunctionFields();
        }

        private void SetTheValue(string theValue)
        {
            if (IsArrayRef)
            {
                _arrayRefItems[_arrayRefItemsModifyIndex] = theValue;
                _arrayRefItems = _arrayRefItems.ToList();
                StateAndResultJSONReload?.Invoke();
            }
            IsArrayRef = false;

            if (_isFnRef)
            {
                ReflectionHelper.SetPropValue(_instanceRef, _propName, theValue);
                StateAndResultJSONReload?.Invoke();
            }
            _isFnRef = false;
        }

        public string GetValue()
        {
            if (SelectedType == STORE_KEY)
            {
                return SelectedStoreKey;
            }
            else if (SelectedType == DEPENDENT_FUNCTION)
            {
                return SelectedDependentFunction;
            }
            else if (SelectedType == INDEPENDENT_FUNCTION)
            {
                return SelectedInDependentFunction;
            }
            return "";
        }

        private void ResetFunctionFields()
        {
            ShowDependentFn = false;
            DependentFnField = string.Empty;

            ShowInDependentFn = false;
            DependentFnField = string.Empty;
        }
        #endregion

        #region From Outside

        // Dependent
        public bool ShowDependentFn { get; set; }

        public string? DependentFnField { get; set; }

        // Independent
        public bool ShowInDependentFn { get; set; }

        public string? InDependentFnField { get; set; }

        private bool IsArrayRef = false;
        private List<string>? _arrayRefItems;
        private int _arrayRefItemsModifyIndex;

        public SmartValuePicker SetTheRefOfArray(List<string>? items, int modifyItemIndex)
        {
            IsArrayRef = true;
            _arrayRefItems = items;
            _arrayRefItemsModifyIndex = modifyItemIndex;
            return this;
        }

        public async Task RemoveValue(List<string> list, int index)
        {
            Console.WriteLine("Removing a Value.");
            list.RemoveAt(index);
            await Task.CompletedTask;
            StateAndResultJSONReload?.Invoke();
            Console.WriteLine("Value Removed.");
        }

        public bool _isFnRef = false;
        public dynamic? _instanceRef;
        public string? _propName;
        public object? _value;

        public SmartValuePicker SetFnParam(dynamic instance, string propName)
        {
            _isFnRef = true;
            this.SetObject(instance, propName);
            return this;
        }

        public SmartValuePicker SetObject(dynamic instance, string propName)
        {
            _instanceRef = instance;
            _propName = propName;
            this.SetFunctionFields();
            return this;
        }

        public async Task OpenValuePicker()
        {
            SelectedType = string.Empty;
            ModalDialog?.Open();
            await Task.CompletedTask;
        }

        public async Task ResetTheValue(dynamic instance, string propName)
        {
            _isFnRef = true;
            this.SetObject(instance, propName);
            this.SetTheValue("");
            await Task.CompletedTask;
        }

        public async Task ClearValue(List<string> list, int index)
        {
            Console.WriteLine("Clearing the Value.");
            list[index] = string.Empty;
            await Task.CompletedTask;
            StateAndResultJSONReload?.Invoke();
            Console.WriteLine("Value Cleared.");
        }

        private void SetFunctionFields()
        {
            // dependent
            PropertyInfo? propertyInfo = _instanceRef?.GetType().GetProperty(_propName);
            Attribute? defaultAttr = propertyInfo?.GetCustomAttribute(typeof(CanUseDependentFunction));
            if (defaultAttr != null)
            {
                ShowDependentFn = true;
                var dependsToField = (defaultAttr as CanUseDependentFunction)?._referToField;

                PropertyInfo? dependsToFieldProp = _instanceRef?.GetType().GetProperty(dependsToField);
                var prop_info = ReflectionHelper.GetInstructionArgumentsNameDescription(dependsToFieldProp);
                DependentFnField = prop_info.Item1;
            }

            // independent
            Attribute? indpndnt_fn = propertyInfo?.GetCustomAttribute(typeof(CanUseInDependentFunction));
            if (indpndnt_fn != null)
            {
                var prop_info = ReflectionHelper.GetInstructionArgumentsNameDescription(propertyInfo);
                ShowInDependentFn = true;
                InDependentFnField = prop_info.Item1;
            }
        }

        public bool PropValueWasSetUsingSmartValuePicker(string property)
        {
            return StoreKeys.Contains(property)
                    || DependentFunctions.ContainsKey(property)
                    || InDependentFunctions.ContainsKey(property);
        }
        #endregion
    }
}
