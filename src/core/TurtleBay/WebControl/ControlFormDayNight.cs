using System;
using TurtleBay.Model;
using WebExpress.UI.WebControl;
using WebExpress.WebHtml;
using WebExpress.WebPage;

namespace TurtleBay.WebControl
{
    public class ControlFormDayNight : ControlForm
    {
        /// <summary>
        /// Liefert oder setzt das Tagesbeginn
        /// </summary>
        private ControlFormItemInputComboBox DayFromCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt das Tagesende
        /// </summary>
        private ControlFormItemInputComboBox DayTillCtrl { get; set; }


        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFormDayNight()
            : base("daynight")
        {
        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <param name="context">Der Kontext, indem das Steuerelement dargestellt wird</param>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode Render(RenderContext context)
        {
            Name = "daynight";
            Margin = new PropertySpacingMargin(PropertySpacing.Space.Three);

            DayFromCtrl = new ControlFormItemInputComboBox()
            {
                Name = "DayFrom",
                Label = "turtlebay:turtlebay.setting.daynight.from.label"
            };

            DayTillCtrl = new ControlFormItemInputComboBox()
            {
                Name = "DayTill",
                Label = "turtlebay:turtlebay.setting.daynight.till.label"
            };

            if (ViewModel.Instance.Solarcalendar.Count > 0)
            {
                DayFromCtrl.Items.Add(new ControlFormItemInputComboBoxItem()
                {
                    Text = "turtlebay:turtlebay.setting.daynight.sunrise.label",
                    Value = string.Format("-1")
                });

                DayTillCtrl.Items.Add(new ControlFormItemInputComboBoxItem()
                {
                    Text = "turtlebay:turtlebay.setting.daynight.sunset.label",
                    Value = string.Format("-1")
                });
            }

            // Werte festlegen
            for (var i = 0; i < 24; i++)
            {
                DayFromCtrl.Items.Add(new ControlFormItemInputComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                DayTillCtrl.Items.Add(new ControlFormItemInputComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });
            }

            Add(DayFromCtrl);
            Add(DayTillCtrl);

            FillFormular += (s, e) =>
            {
                DayFromCtrl.Value = ViewModel.Instance.Settings.DayFrom.ToString();
                DayTillCtrl.Value = ViewModel.Instance.Settings.DayTill.ToString();
            };

            ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.DayFrom = Convert.ToInt32(DayFromCtrl.Value);
                ViewModel.Instance.Settings.DayTill = Convert.ToInt32(DayTillCtrl.Value);
                ViewModel.Instance.SaveSettings();
            };

            DayFromCtrl.Validation += (s, e) =>
            {
                try
                {
                    var dayfrom = Convert.ToInt32(e.Value);
                    var daytill = Convert.ToInt32(DayTillCtrl.Value);

                    if (dayfrom < -1 || dayfrom > 24)
                    {
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "turtlebay:turtlebay.setting.daynight.validation.invalid"));
                    }

                    if (dayfrom > daytill && daytill != -1)
                    {
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Der Tagesanfang darf nicht nach dem Tagesende liegen"));
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult(TypesInputValidity.Error, ex.Message));
                }
            };

            DayTillCtrl.Validation += (s, e) =>
            {
                try
                {
                    var dayfrom = Convert.ToInt32(DayFromCtrl.Value);
                    var daytill = Convert.ToInt32(e.Value);

                    if (daytill < -1 || daytill > 24)
                    {
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "turtlebay:turtlebay.setting.daynight.validation.invalid"));
                    }

                    if (dayfrom > daytill && daytill != -1)
                    {
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Das Tagesende darf nicht vor dem Tagesbeginn liegen"));
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult(TypesInputValidity.Error, ex.Message));
                }
            };

            return base.Render(context);
        }
    }
}
