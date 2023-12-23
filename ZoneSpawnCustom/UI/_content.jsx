import React from 'react';
import { useDataUpdate } from 'hookui-framework'
import { $Panel } from './_panel'
import $Select from './_select'
import $CheckBox from './_checkbox'

const panelID = "zoinkcn.zonespawncustom";

const $DataPage = ({ react, debug = false }) => {
    const [rSizesEnabled, setRSizesEnabled] = react.useState(false)
    const [cSizesEnabled, setCSizesEnabled] = react.useState(false)
    const [iSizesEnabled, setISizesEnabled] = react.useState(false)
    const [oSizesEnabled, setOSizesEnabled] = react.useState(false)

    const [rSizes, setRSizes] = react.useState([])
    const [cSizes, setCSizes] = react.useState([])
    const [iSizes, setISizes] = react.useState([])
    const [oSizes, setOSizes] = react.useState([])

    const [rSizeMin, setRSizeMin] = react.useState('')
    const [rSizeMax, setRSizeMax] = react.useState('')
    const [cSizeMin, setCSizeMin] = react.useState('')
    const [cSizeMax, setCSizeMax] = react.useState('')
    const [iSizeMin, setISizeMin] = react.useState('')
    const [iSizeMax, setISizeMax] = react.useState('')
    const [oSizeMin, setOSizeMin] = react.useState('')
    const [oSizeMax, setOSizeMax] = react.useState('')

    const handleREnabledChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r_enabled', s);
            setRSizesEnabled(s);
        }
    }
    const handleRSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r_size_min', s);
            setRSizeMin(s);
        }
    }
    const handleRSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r_size_max', s);
            setRSizeMax(s);
        }
    }

    const handleCEnabledChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c_enabled', s);
            setCSizesEnabled(s);
        }
    }
    const handleCSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c_size_min', s);
            setCSizeMin(s);
        }
    }
    const handleCSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c_size_max', s);
            setCSizeMax(s);
        }
    }

    const handleIEnabledChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_i_enabled', s);
            setISizesEnabled(s);
        }
    }
    const handleISizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_i_size_min', s);
            setISizeMin(s);
        }
    }
    const handleISizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_i_size_max', s);
            setISizeMax(s);
        }
    }

    const handleOEnabledChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_o_enabled', s);
            setOSizesEnabled(s);
        }
    }
    const handleOSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_o_size_min', s);
            setOSizeMin(s);
        }
    }
    const handleOSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_o_size_max', s);
            setOSizeMax(s);
        }
    }

    if (!debug) {
        useDataUpdate(react, "zone_spawn_custom.r_sizes", setRSizes)
        useDataUpdate(react, "zone_spawn_custom.c_sizes", setCSizes)
        useDataUpdate(react, "zone_spawn_custom.i_sizes", setISizes)
        useDataUpdate(react, "zone_spawn_custom.o_sizes", setOSizes)
        useDataUpdate(react, "zone_spawn_custom.r_enabled", setRSizesEnabled)
        useDataUpdate(react, "zone_spawn_custom.r_size_min", setRSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r_size_max", setRSizeMax)
        useDataUpdate(react, "zone_spawn_custom.c_enabled", setCSizesEnabled)
        useDataUpdate(react, "zone_spawn_custom.c_size_min", setCSizeMin)
        useDataUpdate(react, "zone_spawn_custom.c_size_max", setCSizeMax)
        useDataUpdate(react, "zone_spawn_custom.i_enabled", setISizesEnabled)
        useDataUpdate(react, "zone_spawn_custom.i_size_min", setISizeMin)
        useDataUpdate(react, "zone_spawn_custom.i_size_max", setISizeMax)
        useDataUpdate(react, "zone_spawn_custom.o_enabled", setOSizesEnabled)
        useDataUpdate(react, "zone_spawn_custom.o_size_min", setOSizeMin)
        useDataUpdate(react, "zone_spawn_custom.o_size_max", setOSizeMax)
    }


    return <div>
        <div className='info-section_I7V'>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%' }}>
                <h3 style={{ flex: '1' }}>Residential</h3>
                <$CheckBox react={react} style={{ alignSelf: 'center', margin: '10rem' }} checked={rSizesEnabled} onToggle={handleREnabledChange} />
            </div>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                    <div className='field_vGA'>Min</div>
                    <$Select react={react} options={rSizes} selected={rSizeMin} onSelectionChanged={handleRSizeMinChange} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }}></$Select>
                </div>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                    <div className='field_vGA'>Max</div>
                    <$Select react={react} options={rSizes} selected={rSizeMax} onSelectionChanged={handleRSizeMaxChange} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }}></$Select>
                </div>
            </div>
        </div>
        <div className='info-section_I7V'>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%' }}>
                <h3 style={{ flex: '1' }}>Commercial</h3>
                <$CheckBox react={react} style={{ alignSelf: 'center', margin: '10rem' }} checked={cSizesEnabled} onToggle={handleCEnabledChange} />
            </div>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                    <div className='field_vGA'>Min</div>
                    <$Select react={react} options={cSizes} selected={cSizeMin} onSelectionChanged={handleCSizeMinChange} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }}></$Select>
                </div>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                    <div className='field_vGA'>Max</div>
                    <$Select react={react} options={cSizes} selected={cSizeMax} onSelectionChanged={handleCSizeMaxChange} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }}></$Select>
                </div>
            </div>
        </div>
        <div className='info-section_I7V'>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%' }}>
                <h3 style={{ flex: '1' }}>Industrial</h3>
                <$CheckBox react={react} style={{ alignSelf: 'center', margin: '10rem' }} checked={iSizesEnabled} onToggle={handleIEnabledChange} />
            </div>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                    <div className='field_vGA'>Min</div>
                    <$Select react={react} options={iSizes} selected={iSizeMin} onSelectionChanged={handleISizeMinChange} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }}></$Select>
                </div>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                    <div className='field_vGA'>Max</div>
                    <$Select react={react} options={iSizes} selected={iSizeMax} onSelectionChanged={handleISizeMaxChange} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }}></$Select>
                </div>
            </div>
        </div>
        <div className='info-section_I7V'>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%' }}>
                <h3 style={{ flex: '1' }}>Office</h3>
                <$CheckBox react={react} style={{ alignSelf: 'center', margin: '10rem' }} checked={oSizesEnabled} onToggle={handleOEnabledChange} />
            </div>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                    <div className='field_vGA'>Min</div>
                    <$Select react={react} options={oSizes} selected={oSizeMin} onSelectionChanged={handleOSizeMinChange} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }}></$Select>
                </div>
                <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1' }}>
                    <div className='field_vGA'>Max</div>
                    <$Select react={react} options={oSizes} selected={oSizeMax} onSelectionChanged={handleOSizeMaxChange} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }}></$Select>
                </div>
            </div>
        </div>
    </div>
}

export const $App = ({ react, debug = false }) => {
    return <$Panel title="Zone Spawn Custom" react={react} id={panelID} maxHeight='585rem'>
        <$DataPage react={react} debug={debug}></$DataPage>
    </$Panel>
}