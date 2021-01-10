﻿using System;
using TurtleBay.Model;
using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;

namespace TurtleBay.WebControl
{
    public class ControlFormHeating : ControlFormular
    {
        /// <summary>
        /// Liefert oder setzt die minimale Nachttemperatur
        /// </summary>
        private ControlFormularItemInputComboBox NightMinCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die minimale Tagestemperatur
        /// </summary>
        private ControlFormularItemInputComboBox DayMinCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Maximaltemperatur
        /// </summary>
        private ControlFormularItemInputComboBox MaxCtrl { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFormHeating()
            : base("heating")
        {
        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <param name="context">Der Kontext, indem das Steuerelement dargestellt wird</param>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode Render(RenderContext context)
        {
            Name = "heating";
            EnableCancelButton = false;
            Margin = new PropertySpacingMargin(PropertySpacing.Space.Three);

            NightMinCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "NightMin",
                Label = context.I18N("turtlebay.setting.heating.nightmin.label")
            };

            DayMinCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "DayMin",
                Label = context.I18N("turtlebay.setting.heating.daymin.label")
            };

            MaxCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "Max",
                Label = context.I18N("turtlebay.setting.heating.max.label")
            };

            // Werte festlegen
            // Werte festlegen
            for (var i = 0; i < 40; i++)
            {
                NightMinCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format(context.Culture, "{0} °C", i),
                    Value = string.Format(context.Culture, "{0}", i)
                });

                DayMinCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format(context.Culture, "{0} °C", i),
                    Value = string.Format(context.Culture, "{0}", i)
                });

                MaxCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format(context.Culture, "{0} °C", i),
                    Value = string.Format(context.Culture, "{0}", i)
                });
            }

            Add(NightMinCtrl);
            Add(DayMinCtrl);
            Add(MaxCtrl);

            FillFormular += (s, e) =>
            {
                NightMinCtrl.Value = ViewModel.Instance.Settings.NightMin.ToString();
                DayMinCtrl.Value = ViewModel.Instance.Settings.DayMin.ToString();
                MaxCtrl.Value = ViewModel.Instance.Settings.Max.ToString();
            };

            ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.NightMin = Convert.ToInt32(NightMinCtrl.Value);
                ViewModel.Instance.Settings.DayMin = Convert.ToInt32(DayMinCtrl.Value);
                ViewModel.Instance.Settings.Max = Convert.ToInt32(MaxCtrl.Value);
                ViewModel.Instance.SaveSettings();
            };

            NightMinCtrl.Validation += (s, e) =>
            {
                try
                {
                    var max = Convert.ToInt32(MaxCtrl.Value);
                    var nightMin = Convert.ToInt32(e.Value);

                    if (nightMin < 0)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = context.I18N("turtlebay.setting.heating.validation.night.low"),
                            Type = TypesInputValidity.Warning
                        });
                    }
                    else if (nightMin > 40)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = context.I18N("turtlebay.setting.heating.validation.night.high"),
                            Type = TypesInputValidity.Warning
                        });
                    }

                    if (nightMin > max)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = context.I18N("turtlebay.setting.heating.validation.night.max"),
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

            DayMinCtrl.Validation += (s, e) =>
            {
                try
                {
                    var max = Convert.ToInt32(MaxCtrl.Value);
                    var dayMin = Convert.ToInt32(e.Value);

                    if (dayMin < 0)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = context.I18N("turtlebay.setting.heating.validation.day.low"),
                            Type = TypesInputValidity.Warning
                        });
                    }
                    else if (dayMin > 40)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = context.I18N("turtlebay.setting.heating.validation.day.high"),
                            Type = TypesInputValidity.Warning
                        });
                    }

                    if (dayMin > max)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = context.I18N("turtlebay.setting.heating.validation.day.max"),
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

            MaxCtrl.Validation += (s, e) =>
            {
                try
                {
                    var max = Convert.ToInt32(e.Value);

                    if (max < 1)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = context.I18N("turtlebay.setting.heating.validation.max.low"),
                            Type = TypesInputValidity.Error
                        });
                    }
                    else if (max > 40)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = context.I18N("turtlebay.setting.heating.validation.max.high"),
                            Type = TypesInputValidity.Warning
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

            return base.Render(context);
        }
    }
}