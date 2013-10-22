using System;
using System.Collections.Generic;
using System.Linq;
using WeifenLuo.WinFormsUI.Docking;
using XmlFileExplorer.Domain.Validation;

namespace XmlFileExplorer.Forms
{
    public partial class ErrorsForm : DockContent
    {
        private List<ValidationError> _validationErrors;

        public ErrorsForm()
        {
            InitializeComponent();
            ValidationErrors = new List<ValidationError>();
            SetUpColumnDelegates();
        }

        public List<ValidationError> ValidationErrors
        {
            get { return _validationErrors; }
            set
            {
                _validationErrors = value;
                olvValidationErrors.AddObjects(value);
            }
        }

        private void SetUpColumnDelegates()
        {
            colSeverity.AspectGetter = rowObject => ((ValidationError) rowObject).ErrorSeverity;
            colSeverity.AspectToStringConverter = value => String.Empty;
            colSeverity.GroupKeyGetter = rowObject => ((ValidationError) rowObject).ErrorSeverity;
            colSeverity.GroupKeyToTitleConverter = key => ((ErrorSeverity) key).ToString();
            colSeverity.ImageGetter = delegate(object rowObject)
                {
                    switch (((ValidationError) rowObject).ErrorSeverity)
                    {
                        case ErrorSeverity.Error:
                            return "Error";
                        case ErrorSeverity.Warning:
                            return "Warning";
                        default:
                            return "Error";
                    }
                };
        }

        public void Clear()
        {
            olvValidationErrors.Items.Clear();
        }

        internal void SetErrors(IEnumerable<ValidationError> errors)
        {
            _validationErrors = errors.ToList();
            olvValidationErrors.Items.Clear();
            olvValidationErrors.AddObjects(_validationErrors);
        }
    }
}