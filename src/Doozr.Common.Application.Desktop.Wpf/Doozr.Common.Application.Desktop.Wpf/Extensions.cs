using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Doozr.Common.Application.Desktop.Wpf
{
	public static class Extensions
	{
		public static T GetVisualChild<T>(this Visual referenceVisual) where T : Visual
		{
			Visual child = null;
			for (Int32 i = 0; i < VisualTreeHelper.GetChildrenCount(referenceVisual); i++)
			{
				child = VisualTreeHelper.GetChild(referenceVisual, i) as Visual;
				if (child != null && child is T)
				{
					break;
				}
				else if (child != null)
				{
					child = GetVisualChild<T>(child);
					if (child != null && child is T)
					{
						break;
					}
				}
			}
			return child as T;
		}

		public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject parent) where T : DependencyObject
		{
			int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				if (child is T)
					yield return (T)child;

				foreach (var descendant in GetVisualChildren<T>(child))
					yield return descendant;
			}
		}
	}
}
