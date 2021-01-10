using System;
using TurtleBay.Model;
using WebExpress.UI.WebControl;

namespace TurtleBay.WebControl
{
    public class ControlFormDayNight : ControlFormular
    {
        /// <summary>
        /// Liefert oder setzt das Tagesbeginn
        /// </summary>
        private ControlFormularItemInputComboBox DayFromCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt das Tagesende
        /// </summary>
        private ControlFormularItemInputComboBox DayTillCtrl { get; set; }


        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFormDayNight()
            : base("daynight")
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public void Init()
        {
            Name = "daynight";
            EnableCancelButton = false;
            Margin = new PropertySpacingMargin(PropertySpacing.Space.Three);

            DayFromCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "DayFrom",
                Label = "Tagesanfang:"
            };

            DayTillCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "DayTill",
                Label = "Tagesende:"
            };

            if (ViewModel.Instance.Solarcalendar.Count > 0)
            {
                DayFromCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Sonnenaufgang"),
                    Value = string.Format("-1")
                });

                DayTillCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Sonnenuntergang"),
                    Value = string.Format("-1")
                });
            }

            // Werte festlegen
            for (var i = 0; i < 24; i++)
            {
                DayFromCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                DayTillCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
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
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (dayfrom > daytill && daytill != -1)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Der Tagesanfang darf nicht nach dem Tagesende liegen",
                            Type = TypesInputValidity.Error
                        });
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult()
                    {
                        Text = ex.Message,
                        Type = TypesInputValidity.Error
                    });
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
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (dayfrom > daytill && daytill != -1)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Das Tagesende darf nicht vor dem Tagesbeginn liegen",
                            Type = TypesInputValidity.Error
                        });
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult()
                    {
                        Text = ex.Message,
                        Type = TypesInputValidity.Error
                    });
                }
            };
        }
    }
}
