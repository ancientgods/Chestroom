﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using Terraria;
using TShockAPI;

namespace ChestroomPlugin
{
	public static class ItemType
	{
		public static bool IsWeapon(Item i) => (IsMagicWeapon(i) || IsRangedWeapon(i) || IsMeleeWeapon(i) || IsThrownWeapon(i)) && !IsTool(i);
		public static bool IsRangedWeapon(Item i) => i.ranged && i.shoot > 0;
		public static bool IsMeleeWeapon(Item i) => i.melee;
		public static bool IsMagicWeapon(Item i) => i.magic;
		public static bool IsThrownWeapon(Item i) => i.thrown;


		public static bool IsTool(Item i) => IsPickAxe(i) || IsAxe(i);
		public static bool IsPickAxe(Item i) => i.pick > 0;
		public static bool IsAxe(Item i) => i.axe > 0;


		public static bool IsPlaceable(Item i) => IsTile(i) || IsWall(i) || IsPaint(i);
		public static bool IsTile(Item i) => i.consumable && i.createTile != -1;
		public static bool IsWall(Item i) => i.consumable && i.createWall != -1;
		public static bool IsPaint(Item i) => i.paint > 0;


		public static bool IsArmor(Item i) => (i.headSlot != -1 || i.legSlot != -1 || i.bodySlot != -1) && !i.vanity;

		public static bool IsVanity(Item i) => i.vanity;

		public static bool IsAccessory(Item i) => i.accessory && !IsMusicBox(i);

		public static bool IsDye(Item i) => (~i.width & ~i.height) == ~20 && i.maxStack == 99 &&
			!IsPlaceable(i) && !IsWeapon(i) && !IsTool(i) && !IsMineCart(i) && i.useAnimation == 100;

		public static bool IsAmmo(Item i) => i.ammo > 0 && !IsWeapon(i);

		public static bool IsMusicBox(Item i) => i.createTile == 139;

		public static bool IsBait(Item i) => i.bait > 0;

		public static bool IsMineCart(Item i) => i.type >= 3354 && i.type <= 3356;

		public static bool IsLargeGem(Item i) => i.type == 3643 || (i.type >= 1522 && i.type <= 1527);

		public static bool IsMount(Item i) => i.mountType != -1;

		public static void DumpItemTypeDescription()
		{
			List<Item> NoDescription = new List<Item>();
			List<Item> MultiDescription = new List<Item>();
			using (StreamWriter sw = new StreamWriter(File.Open("ItemTypeDump", FileMode.Create)))
			{
				int count = 0;
				int MultipleDescriptions = 0;
				for (int i = 0; i < Chestroom.ItemIds.Length; i++)
				{
					Item itm = TShock.Utils.GetItemById(Chestroom.ItemIds[i]);

					int Dcount;
					string Description = GetDescription(itm, out Dcount);
					if (Dcount > 1)
					{
						MultipleDescriptions++;
						MultiDescription.Add(itm);
					}
					if (Dcount == 0)
						NoDescription.Add(itm);


					if (!string.IsNullOrWhiteSpace(Description))
						count++;
					else
						Description = "~~~~~~~~~[NO DESCRIPTION AVAILABLE] ~~~~~~~~~";

					sw.WriteLine($"{Chestroom.ItemIds[i]} ({itm.Name}) - {Description}");
				}
				sw.WriteLine(new string('-', 30));
				sw.WriteLine($"{count} items out of {Chestroom.ItemIds.Length} got a type description.");
				sw.WriteLine($"{MultipleDescriptions} items have multiple descriptions.");

				sw.WriteLine(new string('-', 30));
				sw.WriteLine($"Multidescription items: {MultiDescription.Count}");
				for (int i = 0; i < MultiDescription.Count; i++)
				{
					int c;
					sw.WriteLine($"{MultiDescription[i].Name} - {GetDescription(MultiDescription[i], out c)}");
				}
				sw.WriteLine(new string('-', 30));
				sw.WriteLine(new string('-', 30));
				sw.WriteLine($"No description items: {NoDescription.Count}");
				for (int i = 0; i < NoDescription.Count; i++)
				{
					sw.WriteLine($"{NoDescription[i].Name}");
				}
			}
		}

		public static string GetDescription(Item itm, out int dcount)
		{
			StringBuilder sb = new StringBuilder();
			dcount = 0;
			if (IsWeapon(itm))
			{
				dcount++;
				sb.Append("Weapon (");
				if (IsRangedWeapon(itm))
					sb.Append("Ranged");
				if (IsMeleeWeapon(itm))
					sb.Append("Melee");
				if (IsMagicWeapon(itm))
					sb.Append("Magic");
				if (IsThrownWeapon(itm))
					sb.Append("ThrownWeapon");
				sb.Append(")");
			}
			if (IsTool(itm))
			{
				dcount++;
				sb.Append("Tool (");
				if (IsPickAxe(itm))
					sb.Append("Pickaxe");
				if (IsAxe(itm))
					sb.Append("Axe");
				sb.Append(")");
			}
			if (IsArmor(itm))
			{
				dcount++;
				sb.Append("Armor");
			}
			if (IsVanity(itm))
			{
				dcount++;
				sb.Append("Vanity");
			}
			if (IsAccessory(itm))
			{
				dcount++;
				sb.Append("Accessory");
			}
			if (IsPlaceable(itm))
			{
				dcount++;
				sb.Append("Placeable (");
				if (IsTile(itm))
					sb.Append("Tile");
				if (IsWall(itm))
					sb.Append("Wall");
				if (IsPaint(itm))
					sb.Append("Paint");
				sb.Append(")");
			}
			if (IsMount(itm))
			{
				dcount++;
				sb.Append("Mount");
			}
			if (IsAmmo(itm))
			{
				dcount++;
				sb.Append("Ammo");
			}
			if (IsLargeGem(itm))
			{
				dcount++;
				sb.Append("Large gem");
			}
			if (IsBait(itm))
			{
				dcount++;
				sb.Append("Bait");
			}
			if (IsDye(itm))
			{
				dcount++;
				sb.Append("Dye");
			}
			if (IsMineCart(itm))
			{
				dcount++;
				sb.Append("Minecart");
			}
			return sb.ToString();
		}
	}
}
