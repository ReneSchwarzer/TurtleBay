using System;
using TurtleBay.Plugin.Model;
using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlFormLighting : ControlPanelFormular
    {
        /// <summary>
        /// Liefert oder setzt die Startzeit 1 der UVB-Lampe
        /// </summary>
        private ControlFormularItemComboBox FromCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Startzeit 2 der UVB-Lampe
        /// </summary>
        private ControlFormularItemComboBox From2Ctrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Endzeit 1 der UVB-Lampe
        /// </summary>
        private ControlFormularItemComboBox TillCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Endzeit 2 der UVB-Lampe
        /// </summary>
        private ControlFormularItemComboBox Till2Ctrl { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFormLighting(IPage page)
            : base(page, "lighting")
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public void Init()
        {
            Name = "lighting";
            EnableCancelButton = false;
            Class = "m-3";

            FromCtrl = new ControlFormularItemComboBox(this)
            {
                Name = "From",
                Label = "Erste Startzeit des Scheinwerfers:"
            };

            TillCtrl = new ControlFormularItemComboBox(this)
            {
                Name = "Till",
                Label = "Erste Endzeit des Scheinwerfers:"
            };

            From2Ctrl = new ControlFormularItemComboBox(this)
            {
                Name = "From2",
                Label = "Zweite Startzeit des Scheinwerfers:"
            };

            Till2Ctrl = new ControlFormularItemComboBox(this)
            {
                Name = "Till2",
                Label = "Zweite Endzeit des Scheinwerfers:"
            };

            if (ViewModel.Instance.Solarcalendar.Count > 0)
            {
                FromCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("Tagesanfang"),
                    Value = string.Format("-1")
                });

                FromCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("Tagesende"),
                    Value = string.Format("-2")
                });

                TillCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("Tagesanfang"),
                    Value = string.Format("-1")
                });

                TillCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("Tagesende"),
                    Value = string.Format("-2")
                });

                From2Ctrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("Tagesanfang"),
                    Value = string.Format("-1")
                });

                From2Ctrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("Tagesende"),
                    Value = string.Format("-2")
                });

                Till2Ctrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("Tagesanfang"),
                    Value = string.Format("-1")
                });

                Till2Ctrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("Tagesende"),
                    Value = string.Format("-2")
                });
            }

            // Werte festlegen
            for (var i = 0; i < 24; i++)
            {
                FromCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                From2Ctrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                TillCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                Till2Ctrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

            }

            Add(FromCtrl);
            Add(TillCtrl);
            Add(From2Ctrl);
            Add(Till2Ctrl);

            InitFormular += (s, e) =>
            {
                FromCtrl.Value = ViewModel.Instance.Settings.From.ToString();
                TillCtrl.Value = ViewModel.Instance.Settings.Till.ToString();
                From2Ctrl.Value = ViewModel.Instance.Settings.From2.ToString();
                Till2Ctrl.Value = ViewModel.Instance.Settings.Till2.ToString();
            };

            ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.From = Convert.ToInt32(FromCtrl.Value);
                ViewModel.Instance.Settings.Till = Convert.ToInt32(TillCtrl.Value);
                ViewModel.Instance.Settings.From2 = Convert.ToInt32(From2Ctrl.Value);
                ViewModel.Instance.Settings.Till2 = Convert.ToInt32(Till2Ctrl.Value);
                ViewModel.Instance.SaveSettings();
            };

            FromCtrl.Validation += (s, e) =>
            {
                try
                {
                    var from = Convert.ToInt32(e.Value);
                    var till = Convert.ToInt32(TillCtrl.Value);

                    if (from < -2 || from > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (from > till && till >= 0)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Der erste Startzeitpunkt des Scheinwerfers darf nicht nach dem ersten Ende liegen",
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

            TillCtrl.Validation += (s, e) =>
            {
                try
                {
                    var from = Convert.ToInt32(FromCtrl.Value);
                    var till = Convert.ToInt32(e.Value);

                    if (till < -2 || till > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (from > till && till >= 0)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Das erste Ende darf nicht vor dem ersten Startzeitpunkt des Scheinwerfers liegen",
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

            From2Ctrl.Validation += (s, e) =>
            {
                try
                {
                    var from = Convert.ToInt32(e.Value);
                    var till = Convert.ToInt32(Till2Ctrl.Value);

                    if (from < -2 || from > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (from > till)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Der zweite Startzeitpunkt des Scheinwerfers darf nicht nach dem zweiten Ende liegen",
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

            Till2Ctrl.Validation += (s, e) =>
            {
                try
                {
                    var from = Convert.ToInt32(From2Ctrl.Value);
                    var till = Convert.ToInt32(e.Value);

                    if (till < 0 || till > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (from > till)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Das zweite Ende darf nicht vor dem zweiten Startzeitpunkt des Scheinwerfers liegen",
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
