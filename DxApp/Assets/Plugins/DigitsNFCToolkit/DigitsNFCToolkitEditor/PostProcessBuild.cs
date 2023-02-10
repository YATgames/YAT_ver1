#if UNITY_IOS
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#endif

namespace DigitsNFCToolkit.Editor
{
	public class PostProcessBuild
	{
#if UNITY_IOS
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                AddNFCReaderUsageDescription(path);
                AddNFCCapability(path);
            }
        }

        private static void AddNFCReaderUsageDescription(string path)
        {
            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;
            rootDict.SetString("NFCReaderUsageDescription", "Reading NFC Tags");

            PlistElementArray felicaSystemCodes = rootDict.CreateArray("com.apple.developer.nfc.readersession.felica.systemcodes");
            felicaSystemCodes.AddString("12FC");

            PlistElementArray iso7816SelectIdentifiers = rootDict.CreateArray("com.apple.developer.nfc.readersession.iso7816.select-identifiers");
            iso7816SelectIdentifiers.AddString("A0000002471001");
            iso7816SelectIdentifiers.AddString("A000000003101001");
            iso7816SelectIdentifiers.AddString("A000000003101002");
            iso7816SelectIdentifiers.AddString("A0000000041010");
            iso7816SelectIdentifiers.AddString("A0000000042010");
            iso7816SelectIdentifiers.AddString("A0000000044010");
            iso7816SelectIdentifiers.AddString("44464D46412E44466172653234313031");
            iso7816SelectIdentifiers.AddString("D2760000850100");
            iso7816SelectIdentifiers.AddString("D2760000850101");
            iso7816SelectIdentifiers.AddString("00000000000000");

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        private static void AddNFCCapability(string path)
        {
            string projectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject project = new PBXProject();
            project.ReadFromFile(projectPath);

            string packageName = UnityEngine.Application.identifier;
            string name = packageName.Substring(packageName.LastIndexOf('.') + 1);
            string entitlementFileName = name + ".entitlements";
            string entitlementPath = Path.Combine(path, entitlementFileName);

#if UNITY_2019_3_OR_NEWER
            ProjectCapabilityManager projectCapabilityManager = new ProjectCapabilityManager(projectPath, entitlementFileName, null, project.GetUnityMainTargetGuid());
#else
            ProjectCapabilityManager projectCapabilityManager = new ProjectCapabilityManager(projectPath, entitlementFileName, PBXProject.GetUnityTargetName());
#endif
            PlistDocument entitlementDocument = AddNFCEntitlement(projectCapabilityManager);
            entitlementDocument.WriteToFile(entitlementPath);

            var projectInfo = projectCapabilityManager.GetType().GetField("project", BindingFlags.NonPublic | BindingFlags.Instance);
            project = (PBXProject)projectInfo.GetValue(projectCapabilityManager);

#if UNITY_2019_3_OR_NEWER
            string target = project.GetUnityMainTargetGuid();
#else
			string target = project.TargetGuidByName(PBXProject.GetUnityTargetName()); //Use this line for older versions of Unity
#endif
            var constructor = typeof(PBXCapabilityType).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[]{typeof(string), typeof(bool), typeof(string), typeof(bool)}, null);
            PBXCapabilityType nfcCapability = (PBXCapabilityType)constructor.Invoke(new object[] { "com.apple.NearFieldCommunicationTagReading", true, "", false });
            project.AddCapability(target, nfcCapability, entitlementFileName);

            projectCapabilityManager.WriteToFile();
        }

        private static PlistDocument AddNFCEntitlement(ProjectCapabilityManager projectCapabilityManager)
        {
            MethodInfo getMethod = projectCapabilityManager.GetType().GetMethod("GetOrCreateEntitlementDoc", BindingFlags.NonPublic | BindingFlags.Instance);
            PlistDocument entitlementDoc = (PlistDocument)getMethod.Invoke(projectCapabilityManager, new object[] { });

            PlistElementDict dictionary = entitlementDoc.root;
            PlistElementArray array = dictionary.CreateArray("com.apple.developer.nfc.readersession.formats");
            array.values.Add(new PlistElementString("TAG"));
            array.values.Add(new PlistElementString("NDEF"));

            return entitlementDoc;
        }
#endif
        }
    }

