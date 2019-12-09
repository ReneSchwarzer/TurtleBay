using System;
using TurtleBay.Plugin.Model;
using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlFormDayNight : ControlPanelFormular
    {
        /// <summary>
        /// Liefert oder setzt das Tagesbeginn
        /// </summary>
        private ControlFormularItemComboBox DayFromCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt das Tagesende
        /// </summary>
        private ControlFormularItemComboBox DayTillCtrl { get; set; }


        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFormDayNight(IPage page)
            : base(page, "daynight")
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
            Class = "m-3";

            DayFromCtrl = new ControlFormularItemComboBox(this)
            {
                Name = "DayFrom",
                Label = "Tagesanfang:"
            };

            DayTillCtrl = new ControlFormularItemComboBox(this)
            {
                Name = "DayTill",
                Label = "Tagesende:"
            };

            // Werte festlegen
            for (var i = 0; i < 24; i++)
            {
                DayFromCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                DayTillCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

            }

            Add(DayFromCtrl);
            Add(DayTillCtrl);

            InitFormular += (s, e) =>
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

                    if (dayfrom < 0 || dayfrom > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (dayfrom > daytill)
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

                    if (daytill < 0 || daytill > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (dayfrom > daytill)
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
