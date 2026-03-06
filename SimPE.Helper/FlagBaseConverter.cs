/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   Copyright (C) 2025 by GramzeSweatshop                                 *
 *   rhiamom@mac.com                                                       *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/

using System;
using System.ComponentModel;
using System.Globalization;

namespace SimPe
{
	/// <summary>
	/// Used for dynamic PropertyGrids using <see cref="SimPe.FlagBase"/> Objects.
	/// </summary>
	public class FlagBaseConverter : System.ComponentModel.ExpandableObjectConverter 
	{
		public override bool CanConvertTo(ITypeDescriptorContext context,
			System.Type destinationType) 
		{
			if (destinationType == typeof(SimPe.FlagBase))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context,
			CultureInfo culture, 
			object value, 
			System.Type destinationType) 
		{
			if (destinationType == typeof(System.String) && 
				value is SimPe.FlagBase)
			{				
				return Helper.MinStrLength(value.ToString(), 16);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context,
			System.Type sourceType) 
		{
			if (sourceType == typeof(string))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context,
			CultureInfo culture, object value) 
		{
			if (value is string) 
			{
				try 
				{				
					ushort s = Convert.ToUInt16((string)value, 2);
					return System.Activator.CreateInstance(context.PropertyDescriptor.PropertyType, new object[] {s});					
				}
				catch 
				{
					throw new ArgumentException(
						"Can not convert '" + (string)value + "'. This is not a valid Flag Value!");						
				}
			}  
			return base.ConvertFrom(context, culture, value);
		}
	}
}
