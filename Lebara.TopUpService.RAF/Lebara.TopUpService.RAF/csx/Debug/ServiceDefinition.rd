<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Lebara.TopUpService.RAF" generation="1" functional="0" release="0" Id="e6981d02-8ccf-4779-a3f1-3c1f9d725588" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Lebara.TopUpService.RAFGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LB:LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapCertificate|LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="LebaraVoipRAF:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapLebaraVoipRAF:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="LebaraVoipRAFInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/MapLebaraVoipRAFInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapLebaraVoipRAF:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapLebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapLebaraVoipRAFInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAFInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="LebaraVoipRAF" generation="1" functional="0" release="0" software="C:\Users\sarath.mohan\Desktop\Project\ggg\Lebara.TopUpService.RAF\Lebara.TopUpService.RAF\csx\Debug\roles\LebaraVoipRAF" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/SW:LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;LebaraVoipRAF&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;LebaraVoipRAF&quot;&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAFInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAFUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAFFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="LebaraVoipRAFUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="LebaraVoipRAFFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="LebaraVoipRAFInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="602d0f1c-3698-4ff0-8be2-28b1f2333d3c" ref="Microsoft.RedDog.Contract\ServiceContract\Lebara.TopUpService.RAFContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="433d0ce9-6f2c-4402-b39d-3f93d098fec0" ref="Microsoft.RedDog.Contract\Interface\LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Lebara.TopUpService.RAF/Lebara.TopUpService.RAFGroup/LebaraVoipRAF:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>