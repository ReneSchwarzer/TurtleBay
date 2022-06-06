using System;
using TurtleBay.Model;
using WebExpress.UI.WebControl;

namespace TurtleBay.WebControl
{
    public class ControlFormLighting : ControlFormular
    {
        /// <summary>
        /// Liefert oder setzt die Startzeit 1 der UVB-Lampe
        /// </summary>
        private ControlFormularItemInputComboBox FromCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Startzeit 2 der UVB-Lampe
        /// </summary>
        private ControlFormularItemInputComboBox From2Ctrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Endzeit 1 der UVB-Lampe
        /// </summary>
        private ControlFormularItemInputComboBox TillCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Endzeit 2 der UVB-Lampe
        /// </summary>
        private ControlFormularItemInputComboBox Till2Ctrl { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFormLighting()
            : base("lighting")
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public void Init()
        {
            Name = "lighting";
            Margin = new PropertySpacingMargin(PropertySpacing.Space.Three);

            FromCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "From",
                Label = "Erste Startzeit des Scheinwerfers:"
            };

            TillCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "Till",
                Label = "Erste Endzeit des Scheinwerfers:"
            };

            From2Ctrl = new ControlFormularItemInputComboBox()
            {
                Name = "From2",
                Label = "Zweite Startzeit des Scheinwerfers:"
            };

            Till2Ctrl = new ControlFormularItemInputComboBox()
            {
                Name = "Till2",
                Label = "Zweite Endzeit des Scheinwerfers:"
            };

            if (ViewModel.Instance.Solarcalendar.Count > 0)
            {
                FromCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Tagesanfang"),
                    Value = string.Format("-1")
                });

                FromCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Tagesende"),
                    Value = string.Format("-2")
                });

                TillCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Tagesanfang"),
                    Value = string.Format("-1")
                });

                TillCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Tagesende"),
                    Value = string.Format("-2")
                });

                From2Ctrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Tagesanfang"),
                    Value = string.Format("-1")
                });

                From2Ctrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Tagesende"),
                    Value = string.Format("-2")
                });

                Till2Ctrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Tagesanfang"),
                    Value = string.Format("-1")
                });

                Till2Ctrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("Tagesende"),
                    Value = string.Format("-2")
                });
            }

            // Werte festlegen
            for (var i = 0; i < 24; i++)
            {
                FromCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                From2Ctrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                TillCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                Till2Ctrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

            }

            Add(FromCtrl);
            Add(TillCtrl);
            Add(From2Ctrl);
            Add(Till2Ctrl);

            FillFormular += (s, e) =>
            {
                FromCtrl.Value = ViewModel.Instance.Settings.Lighting.From.ToString();
                TillCtrl.Value = ViewModel.Instance.Settings.Lighting.Till.ToString();
                From2Ctrl.Value = ViewModel.Instance.Settings.Lighting.From2.ToString();
                Till2Ctrl.Value = ViewModel.Instance.Settings.Lighting.Till2.ToString();
            };

            ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.Lighting.From = Convert.ToInt32(FromCtrl.Value);
                ViewModel.Instance.Settings.Lighting.Till = Convert.ToInt32(TillCtrl.Value);
                ViewModel.Instance.Settings.Lighting.From2 = Convert.ToInt32(From2Ctrl.Value);
                ViewModel.Instance.Settings.Lighting.Till2 = Convert.ToInt32(Till2Ctrl.Value);
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
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Ungültiger Wert"));
                    }

                    if (from > till && till >= 0)
                    {
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Der erste Startzeitpunkt des Scheinwerfers darf nicht nach dem ersten Ende liegen"));
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult(TypesInputValidity.Error, ex.Message));
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
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Ungültiger Wert"));
                    }

                    if (from > till && till >= 0)
                    {
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Das erste Ende darf nicht vor dem ersten Startzeitpunkt des Scheinwerfers liegen"));
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult(TypesInputValidity.Error, ex.Message));
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
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error,"Ungültiger Wert"));
                    }

                    if (from > till)
                    {
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Der zweite Startzeitpunkt des Scheinwerfers darf nicht nach dem zweiten Ende liegen"));
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult(TypesInputValidity.Error, ex.Message));
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
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Ungültiger Wert"));
                    }

                    if (from > till)
                    {
                        e.Results.Add(new ValidationResult(TypesInputValidity.Error, "Das zweite Ende darf nicht vor dem zweiten Startzeitpunkt des Scheinwerfers liegen"));
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult(TypesInputValidity.Error, ex.Message));
                }
            };
        }
    }
}
