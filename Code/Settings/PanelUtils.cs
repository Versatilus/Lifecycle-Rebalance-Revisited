﻿using System;
using UnityEngine;
using ICities;
using ColossalFramework.UI;


namespace LifecycleRebalance
{
    /// <summary>
    /// Utilities for Options Panel UI.
    /// </summary>
    internal static class PanelUtils
    {
        /// <summary>
        /// Adds a tab to a UI tabstrip.
        /// </summary>
        /// <param name="tabStrip">UIT tabstrip to add to</param>
        /// <param name="tabName">Name of this tab</param>
        /// <param name="tabIndex">Index number of this tab</param>
        /// <returns>UIHelper instance for the new tab panel</returns>
        internal static UIPanel AddTab(UITabstrip tabStrip, string tabName, int tabIndex, bool autoLayout = false)
        {
            // Create tab.
            UIButton tabButton = tabStrip.AddTab(tabName);

            // Sprites.
            tabButton.normalBgSprite = "SubBarButtonBase";
            tabButton.disabledBgSprite = "SubBarButtonBaseDisabled";
            tabButton.focusedBgSprite = "SubBarButtonBaseFocused";
            tabButton.hoveredBgSprite = "SubBarButtonBaseHovered";
            tabButton.pressedBgSprite = "SubBarButtonBasePressed";

            // Tooltip.
            tabButton.tooltip = tabName;

            tabStrip.selectedIndex = tabIndex;

            // Force width.
            tabButton.width = 120;

            // Get tab root panel.
            UIPanel rootPanel = tabStrip.tabContainer.components[tabIndex] as UIPanel;

            // Panel setup.
            rootPanel.autoLayout = autoLayout;
            rootPanel.autoLayoutDirection = LayoutDirection.Vertical;
            rootPanel.autoLayoutPadding.top = 5;
            rootPanel.autoLayoutPadding.left = 10;

            return rootPanel;
        }


        /// <summary>
        /// Adds a plain text label to the specified UI panel.
        /// </summary>
        /// <param name="panel">UI panel to add the label to</param>
        /// <param name="text">Label text</param>
        /// <returns></returns>
        public static UILabel AddLabel(UIPanel panel, string text, float scale = 1.0f)
        {
            // Add label.
            UILabel label = (UILabel)panel.AddUIComponent<UILabel>();
            label.autoSize = false;
            label.autoHeight = true;
            label.wordWrap = true;
            label.width = 700;
            label.textScale = scale;
            label.text = text;

            // Increase panel height to compensate.
            panel.height += label.height;

            return label;
        }





        /// <summary>
        /// Adds a slider with a descriptive text label above and an automatically updating value label immediately to the right.
        /// </summary>
        /// <param name="parent">Panel to add the control to</param>
        /// <param name="text">Descriptive label text</param>
        /// <param name="min">Slider minimum value</param>
        /// <param name="max">Slider maximum value</param>
        /// <param name="step">Slider minimum step</param>
        /// <param name="defaultValue">Slider initial value</param>
        /// <param name="eventCallback">Slider event handler</param>
        /// <param name="width">Slider width (excluding value label to right) (default 600)</param>
        /// <returns>New UI slider with attached labels</returns>
        internal static UISlider AddSliderWithValue(UIComponent parent, string text, float min, float max, float step, float defaultValue, OnValueChanged eventCallback, float width = 600f, float textScale = 1f)
        {
            // Add slider component.
            UIPanel sliderPanel = parent.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsSliderTemplate")) as UIPanel;
            sliderPanel.Find<UILabel>("Label").text = text;

            // Label.
            UILabel sliderLabel = sliderPanel.Find<UILabel>("Label");
            sliderLabel.autoHeight = true;
            sliderLabel.width = width;
            sliderLabel.anchor = UIAnchorStyle.Left | UIAnchorStyle.Top;
            sliderLabel.relativePosition = Vector3.zero;
            sliderLabel.relativePosition = Vector3.zero;
            sliderLabel.textScale = textScale;
            sliderLabel.text = text;

            // Slider configuration.
            UISlider newSlider = sliderPanel.Find<UISlider>("Slider");
            newSlider.minValue = min;
            newSlider.maxValue = max;
            newSlider.stepSize = step;
            newSlider.value = defaultValue;

            // Move default slider position to match resized label.
            sliderPanel.autoLayout = false;
            newSlider.anchor = UIAnchorStyle.Left | UIAnchorStyle.Top;
            newSlider.relativePosition = PositionUnder(sliderLabel);
            newSlider.width = width;

            // Increase height of panel to accomodate it all plus some extra space for margin.
            sliderPanel.autoSize = false;
            sliderPanel.width = width + 50f;
            sliderPanel.height = newSlider.relativePosition.y + newSlider.height + 15f;

            // Value label.
            UILabel valueLabel = sliderPanel.AddUIComponent<UILabel>();
            valueLabel.name = "ValueLabel";
            valueLabel.text = newSlider.value.ToString();
            valueLabel.relativePosition = PositionRightOf(newSlider, 8f, 1f);

            // Event handler to update value label.
            newSlider.eventValueChanged += (component, value) =>
            {
                valueLabel.text = value.ToString();
                eventCallback(value);
            };

            return newSlider;
        }


        /// <summary>
        /// Creates a pushbutton.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="text">Text label</param>
        /// <param name="width">Width (default 100)</param>
        /// <param name="xPos">Relative x position (default 0)</param>
        /// <param name="yPos">Relative y position (default 0)</param>
        /// <returns></returns>
        internal static UIButton CreateButton(UIComponent parent, string text, float width = 200f, float xPos = 0f, float yPos = 0f)
        {
            // Constants.
            const float Height = 30f;


            // Create button.
            UIButton button = parent.AddUIComponent<UIButton>();
            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.disabledTextColor = new Color32(128, 128, 128, 255);
            button.canFocus = false;

            // Button size parameters.
            button.relativePosition = new Vector3(xPos, yPos);
            button.size = new Vector2(width, Height);
            button.textScale = 1f;

            // Label.
            button.text = text;

            return button;
        }


        /// <summary>
        /// Creates a plain checkbox using the game's option panel checkbox template.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="text">Descriptive label text</param>
        /// <returns></returns>
        internal static UICheckBox AddPlainCheckBox(UIComponent parent, string text)
        {
            UICheckBox checkBox = parent.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsCheckBoxTemplate")) as UICheckBox;

            // Set text.
            checkBox.text = text;

            return checkBox;
        }


        /// <summary>
        /// Creates a plain dropdown using the game's option panel dropdown template.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="text">Descriptive label text</param>
        /// <param name="items">Dropdown menu item list</param>
        /// <param name="selectedIndex">Initially selected index (default 0)</param>
        /// <param name="width">Width of dropdown (default 60)</param>
        /// <returns></returns>
        internal static UIDropDown AddPlainDropDown(UIComponent parent, string text, string[] items, int selectedIndex = 0, float width = 270f)
        {
            UIPanel panel = parent.AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsDropdownTemplate")) as UIPanel;
            UIDropDown dropDown = panel.Find<UIDropDown>("Dropdown");

            // Set text.
            panel.Find<UILabel>("Label").text = text;

            // Slightly increase width.
            dropDown.autoSize = false;
            dropDown.width = width;

            // Add items.
            dropDown.items = items;
            dropDown.selectedIndex = selectedIndex;

            return dropDown;
        }


        /// <summary>
        /// Creates a blank options-panel spacer.
        /// </summary>
        /// <param name="parent">Parent component</param>
        /// <param name="parent">Spacer height (default 35)</param>
        /// <returns></returns>
        public static UIPanel AddPanelSpacer(UIComponent parent, float height = 35f)
        {
            UIPanel panel = parent.AddUIComponent<UIPanel>();

            panel.autoSize = false;
            panel.height = height;
            panel.width = parent.width - (panel.relativePosition.x * 2);
            panel.backgroundSprite = "ContentManagerItemBackground";

            return panel;
        }


        /// <summary>
        /// Returns a relative position below a specified UI component, suitable for placing an adjacent component.
        /// </summary>
        /// <param name="uIComponent">Original (anchor) UI component</param>
        /// <param name="margin">Margin between components</param>
        /// <param name="horizontalOffset">Horizontal offset from first to second component</param>
        /// <returns></returns>
        internal static Vector3 PositionUnder(UIComponent uIComponent, float margin = 8f, float horizontalOffset = 0f)
        {
            return new Vector3(uIComponent.relativePosition.x + horizontalOffset, uIComponent.relativePosition.y + uIComponent.height + margin);
        }


        /// <summary>
        /// Returns a relative position to the right of a specified UI component, suitable for placing an adjacent component.
        /// </summary>
        /// <param name="uIComponent">Original (anchor) UI component</param>
        /// <param name="margin">Margin between components</param>
        /// <param name="verticalOffset">Vertical offset from first to second component</param>
        /// <returns></returns>
        internal static Vector3 PositionRightOf(UIComponent uIComponent, float margin = 10f, float verticalOffset = 0f)
        {
            return new Vector3(uIComponent.relativePosition.x + uIComponent.width + margin, uIComponent.relativePosition.y + verticalOffset);
        }


        /// <summary>
        /// Updates XML configuration file with current settings.
        /// </summary>
        internal static void SaveXML()
        {
            // Write to file.
            try
            {
                WG_XMLBaseVersion xml = new XML_VersionTwo();
                xml.writeXML(Loading.currentFileLocation);
            }
            catch (Exception e)
            {
                Debugging.LogException(e);
            }
        }
    }
}