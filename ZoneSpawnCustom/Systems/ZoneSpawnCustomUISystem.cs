using Colossal.UI.Binding;
using Game.UI;
using System;
using ZoneSpawnCustom.Classes;

namespace ZoneSpawnCustom.Systems
{
    public class ZoneSpawnCustomUISystem : UISystemBase
    {
        private string kGroup = "zone_spawn_custom";
        protected override void OnCreate()
        {
            base.OnCreate();
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "r_sizes", () => { return Plugin.ResidentialSizes.Keys.ToWritableList(); }, new ValueWriter<WritableList<string>>()));
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "c_sizes", () => { return Plugin.CommercialSizes.Keys.ToWritableList(); }, new ValueWriter<WritableList<string>>()));
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "i_sizes", () => { return Plugin.IndustrialSizes.Keys.ToWritableList(); }, new ValueWriter<WritableList<string>>()));
            AddUpdateBinding(new GetterValueBinding<WritableList<string>>(kGroup, "o_sizes", () => { return Plugin.OfficeSizes.Keys.ToWritableList(); }, new ValueWriter<WritableList<string>>()));
            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "r_enabled", () => { return Plugin.m_rEnabled.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r_size_min", () => { return Plugin.m_rMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "r_size_max", () => { return Plugin.m_rMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "c_enabled", () => { return Plugin.m_cEnabled.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c_size_min", () => { return Plugin.m_cMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "c_size_max", () => { return Plugin.m_cMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "i_enabled", () => { return Plugin.m_iEnabled.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "i_size_min", () => { return Plugin.m_iMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "i_size_max", () => { return Plugin.m_iMaxSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<bool>(kGroup, "o_enabled", () => { return Plugin.m_oEnabled.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "o_size_min", () => { return Plugin.m_oMinSize.Value; }));
            AddUpdateBinding(new GetterValueBinding<string>(kGroup, "o_size_max", () => { return Plugin.m_oMaxSize.Value; }));

            AddBinding(new TriggerBinding<bool>(kGroup, "set_r_enabled", new Action<bool>(s => Plugin.m_rEnabled.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r_size_min", new Action<string>(s => Plugin.m_rMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_r_size_max", new Action<string>(s => Plugin.m_rMaxSize.Value = s)));
            AddBinding(new TriggerBinding<bool>(kGroup, "set_c_enabled", new Action<bool>(s => Plugin.m_cEnabled.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c_size_min", new Action<string>(s => Plugin.m_cMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_c_size_max", new Action<string>(s => Plugin.m_cMaxSize.Value = s)));
            AddBinding(new TriggerBinding<bool>(kGroup, "set_i_enabled", new Action<bool>(s => Plugin.m_iEnabled.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_i_size_min", new Action<string>(s => Plugin.m_iMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_i_size_max", new Action<string>(s => Plugin.m_iMaxSize.Value = s)));
            AddBinding(new TriggerBinding<bool>(kGroup, "set_o_enabled", new Action<bool>(s => Plugin.m_oEnabled.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_o_size_min", new Action<string>(s => Plugin.m_oMinSize.Value = s)));
            AddBinding(new TriggerBinding<string>(kGroup, "set_o_size_max", new Action<string>(s => Plugin.m_oMaxSize.Value = s)));
        }
    }
}
