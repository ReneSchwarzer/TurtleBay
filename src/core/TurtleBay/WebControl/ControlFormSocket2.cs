﻿using System;
using TurtleBay.Model;
using WebExpress.UI.WebControl;

namespace TurtleBay.WebControl
{
    public class ControlFormSocket2 : ControlFormular
    {
        /// <summary>
        /// Liefert oder setzt den Namen der Steckdose
        /// </summary>
        private ControlFormularItemInputTextBox NameCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Startzeit 1 der Steckdose
        /// </summary>
        private ControlFormularItemInputComboBox FromCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Startzeit 2 der Steckdose
        /// </summary>
        private ControlFormularItemInputComboBox From2Ctrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Endzeit 1 der Steckdose
        /// </summary>
        private ControlFormularItemInputComboBox TillCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Endzeit 2 der Steckdose
        /// </summary>
        private ControlFormularItemInputComboBox Till2Ctrl { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFormSocket2()
            : base("socket2")
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public void Init()
        {
            Name = "socket2";
            EnableCancelButton = false;
            Margin = new PropertySpacingMargin(PropertySpacing.Space.Three);

            NameCtrl = new ControlFormularItemInputTextBox()
            {
                Name = "Name",
                Label = "Der Name der Steckdose:"
            };

            FromCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "From",
                Label = "Erste Startzeit der Steckdose:"
            };

            TillCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "Till",
                Label = "Erste Endzeit der Steckdose:"
            };

            From2Ctrl = new ControlFormularItemInputComboBox()
            {
                Name = "From2",
                Label = "Zweite Startzeit der Steckdose:"
            };

            Till2Ctrl = new ControlFormularItemInputComboBox()
            {
                Name = "Till2",
                Label = "Zweite Endzeit der Steckdose:"
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

            Add(NameCtrl);
            Add(FromCtrl);
            Add(TillCtrl);
            Add(From2Ctrl);
            Add(Till2Ctrl);

            FillFormular += (s, e) =>
            {
                NameCtrl.Value = ViewModel.Instance.Settings.Socket2.Name?.ToString();
                FromCtrl.Value = ViewModel.Instance.Settings.Socket2.From.ToString();
                TillCtrl.Value = ViewModel.Instance.Settings.Socket2.Till.ToString();
                From2Ctrl.Value = ViewModel.Instance.Settings.Socket2.From2.ToString();
                Till2Ctrl.Value = ViewModel.Instance.Settings.Socket2.Till2.ToString();
            };

            ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.Socket2.Name = NameCtrl.Value;
                ViewModel.Instance.Settings.Socket2.From = Convert.ToInt32(FromCtrl.Value);
                ViewModel.Instance.Settings.Socket2.Till = Convert.ToInt32(TillCtrl.Value);
                ViewModel.Instance.Settings.Socket2.From2 = Convert.ToInt32(From2Ctrl.Value);
                ViewModel.Instance.Settings.Socket2.Till2 = Convert.ToInt32(Till2Ctrl.Value);
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
                            Text = "Der erste Startzeitpunkt der Steckdose darf nicht nach dem ersten Ende liegen",
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
                            Text = "Das erste Ende darf nicht vor dem ersten Startzeitpunkt der Steckdose liegen",
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
                            Text = "Der zweite Startzeitpunkt der Steckdose darf nicht nach dem zweiten Ende liegen",
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
                            Text = "Das zweite Ende darf nicht vor dem zweiten Startzeitpunkt der Steckdose liegen",
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