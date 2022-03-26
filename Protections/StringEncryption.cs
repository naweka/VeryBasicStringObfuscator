using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ExmapleObfuscator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExmapleObfuscator.Protections
{
	public class StringsEncryption : IProtection
	{
		private MethodDef methodStringGet;

		public void Run(ModuleDefMD mod)
		{
			Console.WriteLine("Шифруем строки...");

			/*
             * Здесь мы инжектим рантайм метод для расшифровки строк
             */
			var modStringDecryptor = ModuleDefMD.Load(typeof(StrDec).Module);
			var typeStringDecryptor = modStringDecryptor.ResolveTypeDef(MDToken.ToRID(typeof(StrDec).MetadataToken));

			var t = mod.GlobalType;

			var typemembers = InjectHelper.InjectHelper.Inject(typeStringDecryptor, t, mod);
			methodStringGet = (MethodDef)typemembers.Where((IDnlibDef x) => x.Name == "MamaMia").FirstOrDefault();

			foreach (IDnlibDef dnlibDef in typemembers)
				dnlibDef.Name = Guid.NewGuid().ToString().ToUpper().Replace("-", "");

			/*
			 * Конец блока инжекта
			 */



			#region Шифрование
			for (int q = 0; q < mod.Types.Count; q++)
			{
				TypeDef typeDef = mod.Types[q];
				if (typeDef == mod.GlobalType)
					continue;

				for (int j = 0; j < typeDef.Methods.Count; j++)
				{
					MethodDef methodDef = typeDef.Methods[j];
					if (methodDef.Name.StartsWith("get_")
						|| methodDef.Name.StartsWith("set_")
						|| !methodDef.HasBody
						)
						continue;

					for (int instr_i = 0; instr_i < methodDef.Body.Instructions.Count; instr_i++)
					{
						var instr = methodDef.Body.Instructions[instr_i];
						if (instr.OpCode != OpCodes.Ldstr)
							continue;

						var originalString = instr.Operand.ToString();
						if (originalString is null)
							continue;
						var encryptedString = EncryptBytes(originalString);

						// Создаем прокси
						var implFlags = MethodImplAttributes.IL;
						var flags = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static | MethodAttributes.HideBySig;
						var mdu = new MethodDefUser(GetRandomString(), MethodSig.CreateStatic(mod.CorLibTypes.String), implFlags, flags);

						mod.GlobalType.Methods.Add(mdu);

						mdu.Body = new CilBody();
						mdu.Body.Variables.Add(new Local(mod.CorLibTypes.String));

						mdu.Body.Instructions.Add(new Instruction(OpCodes.Ldstr, encryptedString));
						mdu.Body.Instructions.Add(new Instruction(OpCodes.Call, methodStringGet));
						mdu.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));

						instr.OpCode = OpCodes.Call;
						instr.Operand = mdu;
					}
				}
			}
			#endregion
		}

		private static string GetRandomString() => Guid.NewGuid().ToString().ToUpper().Replace("-", "");

		private static string EncryptBytes(string plainText)
		{
			byte[] bytes = Encoding.BigEndianUnicode.GetBytes(plainText);

			byte[] initVectorBytes = Encoding.UTF8.GetBytes("wnegofn93bwubdinfdmcmn29gqdwm083g4g93");

			for (int i = 0; i < bytes.Length; i++)
			{
				bytes[i] ^= (byte)((initVectorBytes[i % initVectorBytes.Length]) % 255);
			}

			return Convert.ToBase64String(bytes);
		}

		public static class StrDec
		{
			// Достаточно простое шифрование строк 
			// Нет антиэмуля
			// Есть "сопротивление" созданию статик деобфуска (но смысл от этого, если можно спокойно инвокать?)
			public unsafe static string MamaMia(string hrthrtdfb499re8fg4wre9g8e4g84)
			{
				int oguneognee8th49et4hrogerngo68eth49684 = 0;
				int erger1h6y8j4rhy4u98k4i9k84 = 59;
				int svwewet849e4y = 978;
				int rhjk984k98ty4j9er = 0;
				int wrgrj4k98yik4u98k9 = 0;
				int vhr84tyj98yj49hr98g49wrg = 16;
				int ntyjrj89e49489thrth4984ewth = 0;
				int wedfcb8y64j98j4r9jheh = 255;
				if (hrthrtdfb499re8fg4wre9g8e4g84 == null)
				{
					goto sawrg84wr9g84wrg9cgg;
				}
				fixed (char* dfvhrthrt4hrt4h9r8t4h9r8t4hr984 = hrthrtdfb499re8fg4wre9g8e4g84)
				{
					char* hrthju65wr1g6wr14g6rwgykukyjrh = dfvhrthrt4hrt4h9r8t4h9r8t4hr984;
					int fwerthtryhty1498j4y = hrthrtdfb499re8fg4wre9g8e4g84.Length;
					while (fwerthtryhty1498j4y > 0)
					{
						int fwegerhty84jtr96h4169g = (int)hrthju65wr1g6wr14g6rwgykukyjrh[fwerthtryhty1498j4y - 1];
						if (fwegerhty84jtr96h4169g != 32 && fwegerhty84jtr96h4169g != 10 && fwegerhty84jtr96h4169g != 13 && fwegerhty84jtr96h4169g != 9)
						{
							break;
						}
						fwerthtryhty1498j4y--;
					}

					char* brbt5968w1r6g1wr6gynty = hrthju65wr1g6wr14g6rwgykukyjrh;
					int xcvreg = fwerthtryhty1498j4y;
					oguneognee8th49et4hrogerngo68eth49684 = fwerthtryhty1498j4y;
					char* gerw96r4g9wr84g9wrggerg = brbt5968w1r6g1wr6gynty + xcvreg;
					string fjxdgwrg98wr4g9wr8g4wr9rtj56543 = "wnegofn93bwubdinfdmcmn29gqdwm083g4g93";
					char[] sfef9wrg419wr84gwr9g84rwew6566 = fjxdgwrg98wr4g9wr8g4wr9rtj56543.ToCharArray();
					char[] gyjmpgirnge65 = sfef9wrg419wr84gwr9g84rwew6566;
					int rgsengx6c84vbx9c4e9r84hfoigui6 = 0;
					svwewet849e4y = gyjmpgirnge65.Length;
					int weigbtu8k49h41g6wsre4wreei12 = sfef9wrg419wr84gwr9g84rwew6566.Length;
					int trher = 0;
					fixed (char* ntyimoiwr84g9wr8g4t6 = gyjmpgirnge65)
					{
						trher = Encoding.UTF8.GetByteCount(ntyimoiwr84g9wr8g4t6 + rgsengx6c84vbx9c4e9r84hfoigui6, weigbtu8k49h41g6wsre4wreei12);
					}
					byte[] dsfverger6h46 = new byte[trher];
					int bytes = Encoding.UTF8.GetBytes(fjxdgwrg98wr4g9wr8g4wr9rtj56543, 0, fjxdgwrg98wr4g9wr8g4wr9rtj56543.Length, dsfverger6h46, 0);
					byte[] dzvsewrg2wr96g4wr98g4wr9gfwef684 = dsfverger6h46;
					rhjk984k98ty4j9er = dzvsewrg2wr96g4wr98g4wr9gfwef684.Length;
					int yumwrg6r841bh984yj1t8j94ytujuiy = xcvreg;
					erger1h6y8j4rhy4u98k4i9k84 = yumwrg6r841bh984yj1t8j94ytujuiy;
					int cwrg98w4rg98eth49r8t4hvnnyrn = 0;
					while (brbt5968w1r6g1wr6gynty < gerw96r4g9wr84g9wrggerg)
					{
						uint uiui = (uint)(*brbt5968w1r6g1wr6gynty);
						brbt5968w1r6g1wr6gynty++;
						if (uiui <= 32U)
						{
							yumwrg6r841bh984yj1t8j94ytujuiy--;
						}
						else if (uiui == 61U)
						{
							yumwrg6r841bh984yj1t8j94ytujuiy--;
							svwewet849e4y ^= yumwrg6r841bh984yj1t8j94ytujuiy;
							cwrg98w4rg98eth49r8t4hvnnyrn++;
						}
					}
					if (cwrg98w4rg98eth49r8t4hvnnyrn != 0)
					{
						if (cwrg98w4rg98eth49r8t4hvnnyrn == 1)
						{
							cwrg98w4rg98eth49r8t4hvnnyrn = 2;
							oguneognee8th49et4hrogerngo68eth49684++;
						}
						else
						{
							if (cwrg98w4rg98eth49r8t4hvnnyrn != 2)
							{
								goto sawrg84wr9g84wrg9cgg;
							}
							cwrg98w4rg98eth49r8t4hvnnyrn = 1;
							erger1h6y8j4rhy4u98k4i9k84++;
						}
					}
					int nuqew5ferh98rth4rt984jm2 = yumwrg6r841bh984yj1t8j94ytujuiy / 4 * 3 + cwrg98w4rg98eth49r8t4hvnnyrn;
					byte[] ergtyjyt684 = new byte[nuqew5ferh98rth4rt984jm2];
					fixed (byte* ptr2 = ergtyjyt684)
					{
						int scdgergerg4684 = -999999;
						char* startInputPtr22 = hrthju65wr1g6wr14g6rwgykukyjrh;
						int inputLength353 = fwerthtryhty1498j4y;
						byte* startDestPtr34656 = ptr2;
						int destLength62452 = nuqew5ferh98rth4rt984jm2;

						char* dzvserg4er9g6er4gfbdfb = startInputPtr22;
						byte* pirgwrtg98rwt9gre84htrttrh = startDestPtr34656;
						rhjk984k98ty4j9er = oguneognee8th49et4hrogerngo68eth49684 + svwewet849e4y;
						char* xcbyrt48j849k98jh4re98g4wg98wrglkhgf = dzvserg4er9g6er4gfbdfb + inputLength353;
						byte* sdgetnr = pirgwrtg98rwt9gre84htrttrh + destLength62452;

						uint yukjywrg85wr9g8w4rg98rw4uj9684 = 255U;

						while (dzvserg4er9g6er4gfbdfb < xcbyrt48j849k98jh4re98g4wg98wrglkhgf)
						{
							uint rthrthrtj684tyhtyh = (uint)(*dzvserg4er9g6er4gfbdfb);
							dzvserg4er9g6er4gfbdfb++;
							if (rthrthrtj684tyhtyh - 65U <= 25U)
							{
								rthrthrtj684tyhtyh -= 65U;
							}
							else if (rthrthrtj684tyhtyh - 97U <= 25U)
							{
								ntyjrj89e49489thrth4984ewth -= wedfcb8y64j98j4r9jheh;
								rthrthrtj684tyhtyh -= 71U;
							}
							else
							{
								if (rthrthrtj684tyhtyh - 48U > 9U)
								{
									if (rthrthrtj684tyhtyh <= 32U)
									{
										if (rthrthrtj684tyhtyh - 9U <= 1U || rthrthrtj684tyhtyh == 13U || rthrthrtj684tyhtyh == 32U)
										{
											rhjk984k98ty4j9er--;
											continue;
										}
									}
									else
									{
										if (rthrthrtj684tyhtyh == 43U)
										{
											vhr84tyj98yj49hr98g49wrg = rhjk984k98ty4j9er | 8;
											rthrthrtj684tyhtyh = 62U;
											goto IL_A7;
										}
										if (rthrthrtj684tyhtyh == 47U)
										{
											rthrthrtj684tyhtyh = 63U;
											wrgrj4k98yik4u98k9 -= rhjk984k98ty4j9er;
											goto IL_A7;
										}
										if (rthrthrtj684tyhtyh == 61U)
										{
											if (dzvserg4er9g6er4gfbdfb == xcbyrt48j849k98jh4re98g4wg98wrglkhgf)
											{
												yukjywrg85wr9g8w4rg98rw4uj9684 <<= 6;
												if ((yukjywrg85wr9g8w4rg98rw4uj9684 & 2147483648U) == 0U)
												{
													goto sawrg84wr9g84wrg9cgg;
												}
												if ((int)((long)(sdgetnr - pirgwrtg98rwt9gre84htrttrh)) < 2)
												{
													vhr84tyj98yj49hr98g49wrg = 0;
													scdgergerg4684 = -1;
													goto wef4gw98g4r9ger9h84da;
												}
												*(pirgwrtg98rwt9gre84htrttrh++) = (byte)(yukjywrg85wr9g8w4rg98rw4uj9684 >> 16);
												wrgrj4k98yik4u98k9 = erger1h6y8j4rhy4u98k4i9k84 >> 19;
												*(pirgwrtg98rwt9gre84htrttrh++) = (byte)(yukjywrg85wr9g8w4rg98rw4uj9684 >> 8);
												yukjywrg85wr9g8w4rg98rw4uj9684 = 255U;
												break;
											}
											else
											{
												while (dzvserg4er9g6er4gfbdfb < xcbyrt48j849k98jh4re98g4wg98wrglkhgf - 1)
												{
													int mhjmwr84g9wr4gwr984gtym9 = (int)(*dzvserg4er9g6er4gfbdfb);
													rhjk984k98ty4j9er = 0;
													if (mhjmwr84g9wr4gwr984gtym9 != 32 && mhjmwr84g9wr4gwr984gtym9 != 10 && mhjmwr84g9wr4gwr984gtym9 != 13 && mhjmwr84g9wr4gwr984gtym9 != 9)
													{
														break;
													}
													dzvserg4er9g6er4gfbdfb++;
												}
												if (dzvserg4er9g6er4gfbdfb != xcbyrt48j849k98jh4re98g4wg98wrglkhgf - 1 || *dzvserg4er9g6er4gfbdfb != '=')
												{
													rhjk984k98ty4j9er -= 97;
													goto sawrg84wr9g84wrg9cgg;
												}
												yukjywrg85wr9g8w4rg98rw4uj9684 <<= 12;
												if ((yukjywrg85wr9g8w4rg98rw4uj9684 & 2147483648U) == 0U)
												{
													goto sawrg84wr9g84wrg9cgg;
												}
												if ((int)((long)(sdgetnr - pirgwrtg98rwt9gre84htrttrh)) < 1)
												{
													scdgergerg4684 = -1;
													svwewet849e4y = rhjk984k98ty4j9er & erger1h6y8j4rhy4u98k4i9k84 & wedfcb8y64j98j4r9jheh;
													goto wef4gw98g4r9ger9h84da;
												}
												*(pirgwrtg98rwt9gre84htrttrh++) = (byte)(yukjywrg85wr9g8w4rg98rw4uj9684 >> 16);
												yukjywrg85wr9g8w4rg98rw4uj9684 = 255U;
												break;
											}
										}
									}
									goto sawrg84wr9g84wrg9cgg;
								}
								vhr84tyj98yj49hr98g49wrg -= 1024;
								rthrthrtj684tyhtyh -= 4294967292U;
							}
						IL_A7:
							yukjywrg85wr9g8w4rg98rw4uj9684 = (yukjywrg85wr9g8w4rg98rw4uj9684 << 6 | rthrthrtj684tyhtyh);
							if ((yukjywrg85wr9g8w4rg98rw4uj9684 & 2147483648U) != 0U)
							{
								if ((int)((long)(sdgetnr - pirgwrtg98rwt9gre84htrttrh)) < 3)
								{
									oguneognee8th49et4hrogerngo68eth49684++;
									scdgergerg4684 = -1;
									goto wef4gw98g4r9ger9h84da;
								}
								*pirgwrtg98rwt9gre84htrttrh = (byte)(yukjywrg85wr9g8w4rg98rw4uj9684 >> 16);
								wrgrj4k98yik4u98k9 = vhr84tyj98yj49hr98g49wrg >> 8;
								pirgwrtg98rwt9gre84htrttrh[1] = (byte)(yukjywrg85wr9g8w4rg98rw4uj9684 >> 8);
								erger1h6y8j4rhy4u98k4i9k84 |= oguneognee8th49et4hrogerngo68eth49684 + 17;
								pirgwrtg98rwt9gre84htrttrh[2] = (byte)yukjywrg85wr9g8w4rg98rw4uj9684;
								pirgwrtg98rwt9gre84htrttrh += 3;
								yukjywrg85wr9g8w4rg98rw4uj9684 = 255U;
							}
						}
						if (yukjywrg85wr9g8w4rg98rw4uj9684 != 255U)
						{
							goto sawrg84wr9g84wrg9cgg;
						}
						scdgergerg4684 = (int)((long)(pirgwrtg98rwt9gre84htrttrh - startDestPtr34656));
						goto wef4gw98g4r9ger9h84da;

					wef4gw98g4r9ger9h84da:;
					}

					byte[] nhytyjtq6e4f9qe4f9qefyj6ty16 = ergtyjyt684;


					for (int i = 0; i < nhytyjtq6e4f9qe4f9qefyj6ty16.Length; i++)
					{
						nhytyjtq6e4f9qe4f9qefyj6ty16[i] ^= (byte)((dzvsewrg2wr96g4wr98g4wr9gfwef684[i % dzvsewrg2wr96g4wr98g4wr9gfwef684.Length]) % 255);
					}
					byte[] sdcsdc6 = nhytyjtq6e4f9qe4f9qefyj6ty16;
					svwewet849e4y = rhjk984k98ty4j9er - 1;
					int oiw69r84gw9r84gwr98g4n895 = 0;
					int wrgreg684 = nhytyjtq6e4f9qe4f9qefyj6ty16.Length;
					if (sdcsdc6.Length == 0)
					{
						return "";
					}
					fixed (byte* bgrthhtyj = sdcsdc6)
					{
						char[] fbd98w4rg9wr4g98wr46wr849redh4rt98h4 = new char[Encoding.BigEndianUnicode.GetCharCount(bgrthhtyj + oiw69r84gw9r84gwr98g4n895, wrgreg684)];
						byte[] thntyjyukj895 = nhytyjtq6e4f9qe4f9qefyj6ty16;
						int dfvx6w4rg96w4r89gw4r6w4698grcv4 = 0;
						int zcbbfsdbfbdfh14t9rh41tr96h = nhytyjtq6e4f9qe4f9qefyj6ty16.Length;
						wedfcb8y64j98j4r9jheh = erger1h6y8j4rhy4u98k4i9k84 - 255;
						char[] vswrg56w1r9gw4rg9w84rgwr6g4wrgdv651 = fbd98w4rg9wr4g98wr46wr849redh4rt98h4;
						int iohwr984gwr98g4wr98g4wrlh99 = 0;
						if (thntyjyukj895.Length == 0)
						{
							return "";
						}
						int ggv84wr9gw4r98gw4rg9wr9g84dbrhr651 = vswrg56w1r9gw4rg9w84rgwr6g4wrgdv651.Length - iohwr984gwr98g4wr98g4wrlh99;
						if (vswrg56w1r9gw4rg9w84rgwr6g4wrgdv651.Length == 0)
						{
							svwewet849e4y = 1;
							vswrg56w1r9gw4rg9w84rgwr6g4wrgdv651 = new char[1];
						}
						fixed (byte* per4e9r4t9er84treqwe = thntyjyukj895)
						{
							fixed (char* wefer9g8494ty98j4yuj = vswrg56w1r9gw4rg9w84rgwr6g4wrgdv651)
							{
								rhjk984k98ty4j9er = svwewet849e4y << 17;
								Encoding.BigEndianUnicode.GetChars(per4e9r4t9er84treqwe + dfvx6w4rg96w4r89gw4r6w4698grcv4, zcbbfsdbfbdfh14t9rh41tr96h, wefer9g8494ty98j4yuj + iohwr984gwr98g4wr98g4wrlh99, ggv84wr9gw4r98gw4rg9wr9g84dbrhr651);
							}
						}

						return new string(fbd98w4rg9wr4g98wr46wr849redh4rt98h4);
					}

				}
			sawrg84wr9g84wrg9cgg:;
				throw new Exception();
			}
		}
	}
}
