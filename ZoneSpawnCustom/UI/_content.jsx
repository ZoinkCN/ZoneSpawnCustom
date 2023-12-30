import React from 'react';
import { useDataUpdate } from 'hookui-framework'
import { $Panel } from './_panel'
import $Select from './_select'
import $CheckBox from './_checkbox'
import $TabControl from './_tab-control'

const panelID = "zoinkcn.zonespawncustom";

const $ThemeTab = ({ react, datas }) => {
    const [minStr, setMinStr] = react.useState('Min');
    const [maxStr, setMaxStr] = react.useState('Max');

    useDataUpdate(react, 'zone_spawn_custom.min', setMinStr);
    useDataUpdate(react, 'zone_spawn_custom.max', setMaxStr);

    return (
        <div style={{ flex: '1' }}>
            {datas.map((item, index) =>
                <div key={index} style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1', alignItems: 'center' }}>
                    <img src={item.icon} alt='' style={{ height: '42.000000rem', width: '42.000000rem', marginLeft: '10.000000rem', marginRight: '5.000000rem' }} />
                    <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1', alignItems: 'center' }}>
                        <div style={{ marginLeft: '6rem' }}>{minStr}</div>
                        <$Select react={react} options={item.minSizes} selected={item.min} onSelectionChanged={item.onMinChnaged} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }} />
                    </div>
                    <div style={{ display: 'flex', flexDirection: 'row', width: '100%', flex: '1', alignItems: 'center' }}>
                        <div >{maxStr}</div>
                        <$Select react={react} options={item.maxSizes} selected={item.max} onSelectionChanged={item.onMaxChanged} style={{ flex: '1', margin: '10rem', minWidth: '10rem' }} />
                    </div>
                </div>
            )}
        </div>
    );
}

const $SettingPage = ({ react, title, enabled, normalData, hasDetail = false, detailDatas = null, detailed = false, hasTheme = false, onEnabledChanged = null, onDetailedChanged = null }) => {
    const [detailStr, setDetailStr] = react.useState('Detail');
    useDataUpdate(react, 'zone_spawn_custom.detail', setDetailStr);
    const normalPage = <$ThemeTab react={react} datas={normalData.data} ></$ThemeTab>

    const detailedPage = hasDetail ? (
        detailDatas ? (
            hasTheme ? (
                <$TabControl react={react} tabs={
                    detailDatas.map(datas => (
                        {
                            name: datas.isEU ? 'EU' : 'NA',
                            icon: datas.isEU ? 'Media/Game/Themes/European.svg' : 'Media/Game/Themes/North American.svg',
                            iconOnly: true,
                            content: (<$ThemeTab react={react} datas={datas.data}></$ThemeTab>)
                        }
                    ))
                }></$TabControl>) : (
                <$ThemeTab react={react} datas={detailDatas[0].data}></$ThemeTab>
            )
        ) : null
    ) : null

    return (
        <div className='info-section_I7V'>
            <div style={{ display: 'flex', flexDirection: 'row', width: '100%', alignItems: 'center' }}>
                <$CheckBox react={react} style={{ alignSelf: 'center', margin: '10rem' }} checked={enabled} onToggle={onEnabledChanged} />
                <h3 style={{ flex: '1', margin: 'auto 15rem' }}>{title}</h3>
                {
                    hasDetail && enabled ? <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center' }}>
                        <h4 style={{ margin: 'auto 5rem auto auto' }}>{detailStr}</h4>
                        <$CheckBox react={react} style={{ alignSelf: 'center', margin: '10rem' }} checked={detailed} onToggle={onDetailedChanged} />
                    </div> : null
                }
            </div>
            {enabled ? <div className='section_sop'>
                {detailed ? detailedPage : normalPage}
            </div> : null}
        </div>
    )
}

const $DataPage = ({ react, debug = false }) => {
    const [rSizesEnabled, setRSizesEnabled] = react.useState(false)
    const [rSizesDetailed, setRSizesDetailed] = react.useState(false)
    const [cSizesEnabled, setCSizesEnabled] = react.useState(false)
    const [cSizesDetailed, setCSizesDetailed] = react.useState(false)
    const [iSizesEnabled, setISizesEnabled] = react.useState(false)
    const [oSizesEnabled, setOSizesEnabled] = react.useState(false)
    const [oSizesDetailed, setOSizesDetailed] = react.useState(false)

    const [rSizes, setRSizes] = react.useState([])
    const [r1EuSizes, setR1EuSizes] = react.useState([])
    const [r2EuSizes, setR2EuSizes] = react.useState([])
    const [r3EuSizes, setR3EuSizes] = react.useState([])
    const [r4EuSizes, setR4EuSizes] = react.useState([])
    const [r6EuSizes, setR6EuSizes] = react.useState([])
    const [r1NaSizes, setR1NaSizes] = react.useState([])
    const [r2NaSizes, setR2NaSizes] = react.useState([])
    const [r3NaSizes, setR3NaSizes] = react.useState([])
    const [r4NaSizes, setR4NaSizes] = react.useState([])
    const [r6NaSizes, setR6NaSizes] = react.useState([])
    const [r5Sizes, setR5Sizes] = react.useState([])

    const [cSizes, setCSizes] = react.useState([])
    const [c1EuSizes, setC1EuSizes] = react.useState([])
    const [c2EuSizes, setC2EuSizes] = react.useState([])
    const [c1NaSizes, setC1NaSizes] = react.useState([])
    const [c2NaSizes, setC2NaSizes] = react.useState([])

    const [iSizes, setISizes] = react.useState([])

    const [oSizes, setOSizes] = react.useState([])
    const [o1Sizes, setO1Sizes] = react.useState([])
    const [o2Sizes, setO2Sizes] = react.useState([])

    const [rSizeMin, setRSizeMin] = react.useState('')
    const [rSizeMax, setRSizeMax] = react.useState('')
    const [r1EuSizeMin, setR1EuSizeMin] = react.useState('')
    const [r1EuSizeMax, setR1EuSizeMax] = react.useState('')
    const [r2EuSizeMin, setR2EuSizeMin] = react.useState('')
    const [r2EuSizeMax, setR2EuSizeMax] = react.useState('')
    const [r3EuSizeMin, setR3EuSizeMin] = react.useState('')
    const [r3EuSizeMax, setR3EuSizeMax] = react.useState('')
    const [r4EuSizeMin, setR4EuSizeMin] = react.useState('')
    const [r4EuSizeMax, setR4EuSizeMax] = react.useState('')
    const [r6EuSizeMin, setR6EuSizeMin] = react.useState('')
    const [r6EuSizeMax, setR6EuSizeMax] = react.useState('')
    const [r1NaSizeMin, setR1NaSizeMin] = react.useState('')
    const [r1NaSizeMax, setR1NaSizeMax] = react.useState('')
    const [r2NaSizeMin, setR2NaSizeMin] = react.useState('')
    const [r2NaSizeMax, setR2NaSizeMax] = react.useState('')
    const [r3NaSizeMin, setR3NaSizeMin] = react.useState('')
    const [r3NaSizeMax, setR3NaSizeMax] = react.useState('')
    const [r4NaSizeMin, setR4NaSizeMin] = react.useState('')
    const [r4NaSizeMax, setR4NaSizeMax] = react.useState('')
    const [r6NaSizeMin, setR6NaSizeMin] = react.useState('')
    const [r6NaSizeMax, setR6NaSizeMax] = react.useState('')
    const [r5SizeMin, setR5SizeMin] = react.useState('')
    const [r5SizeMax, setR5SizeMax] = react.useState('')

    const [cSizeMin, setCSizeMin] = react.useState('')
    const [cSizeMax, setCSizeMax] = react.useState('')
    const [c1EuSizeMin, setC1EuSizeMin] = react.useState('')
    const [c1EuSizeMax, setC1EuSizeMax] = react.useState('')
    const [c2EuSizeMin, setC2EuSizeMin] = react.useState('')
    const [c2EuSizeMax, setC2EuSizeMax] = react.useState('')
    const [c1NaSizeMin, setC1NaSizeMin] = react.useState('')
    const [c1NaSizeMax, setC1NaSizeMax] = react.useState('')
    const [c2NaSizeMin, setC2NaSizeMin] = react.useState('')
    const [c2NaSizeMax, setC2NaSizeMax] = react.useState('')

    const [iSizeMin, setISizeMin] = react.useState('')
    const [iSizeMax, setISizeMax] = react.useState('')

    const [oSizeMin, setOSizeMin] = react.useState('')
    const [oSizeMax, setOSizeMax] = react.useState('')
    const [o1SizeMin, setO1SizeMin] = react.useState('')
    const [o1SizeMax, setO1SizeMax] = react.useState('')
    const [o2SizeMin, setO2SizeMin] = react.useState('')
    const [o2SizeMax, setO2SizeMax] = react.useState('')

    const [rStr, setRStr] = react.useState('Residential')
    const [cStr, setCStr] = react.useState('Commercial')
    const [iStr, setIStr] = react.useState('Industrial')
    const [oStr, setOStr] = react.useState('Office')

    const handleREnabledChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r_enabled', s);
            setRSizesEnabled(s);
        }
    }
    const handleRDetailedChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r_detailed', s);
            setRSizesDetailed(s);
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
    const handleR1EuSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r1_eu_size_min', s);
            setR1EuSizeMin(s);
        }
    }
    const handleR1EuSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r1_eu_size_max', s);
            setR1EuSizeMax(s);
        }
    }
    const handleR2EuSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r2_eu_size_min', s);
            setR2EuSizeMin(s);
        }
    }
    const handleR2EuSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r2_eu_size_max', s);
            setR2EuSizeMax(s);
        }
    }
    const handleR3EuSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r3_eu_size_min', s);
            setR3EuSizeMin(s);
        }
    }
    const handleR3EuSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r3_eu_size_max', s);
            setR3EuSizeMax(s);
        }
    }
    const handleR4EuSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r4_eu_size_min', s);
            setR4EuSizeMin(s);
        }
    }
    const handleR4EuSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r4_eu_size_max', s);
            setR4EuSizeMax(s);
        }
    }
    const handleR6EuSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r6_eu_size_min', s);
            setR6EuSizeMin(s);
        }
    }
    const handleR6EuSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r6_eu_size_max', s);
            setR6EuSizeMax(s);
        }
    }
    const handleR1NaSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r1_na_size_min', s);
            setR1NaSizeMin(s);
        }
    }
    const handleR1NaSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r1_na_size_max', s);
            setR1NaSizeMax(s);
        }
    }
    const handleR2NaSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r2_na_size_min', s);
            setR2NaSizeMin(s);
        }
    }
    const handleR2NaSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r2_na_size_max', s);
            setR2NaSizeMax(s);
        }
    }
    const handleR3NaSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r3_na_size_min', s);
            setR3NaSizeMin(s);
        }
    }
    const handleR3NaSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r3_na_size_max', s);
            setR3NaSizeMax(s);
        }
    }
    const handleR4NaSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r4_na_size_min', s);
            setR4NaSizeMin(s);
        }
    }
    const handleR4NaSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r4_na_size_max', s);
            setR4NaSizeMax(s);
        }
    }
    const handleR6NaSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r6_na_size_min', s);
            setR6NaSizeMin(s);
        }
    }
    const handleR6NaSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r6_na_size_max', s);
            setR6NaSizeMax(s);
        }
    }
    const handleR5SizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r5_size_min', s);
            setR5SizeMin(s);
        }
    }
    const handleR5SizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_r5_size_max', s);
            setR5SizeMax(s);
        }
    }

    const handleCEnabledChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c_enabled', s);
            setCSizesEnabled(s);
        }
    }
    const handleCDetailedChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c_detailed', s);
            setCSizesDetailed(s);
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
    const handleC1EuSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c1_eu_size_min', s);
            setC1EuSizeMin(s);
        }
    }
    const handleC1EuSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c1_eu_size_max', s);
            setC1EuSizeMax(s);
        }
    }
    const handleC2EuSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c2_eu_size_min', s);
            setC2EuSizeMin(s);
        }
    }
    const handleC2EuSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c2_eu_size_max', s);
            setC2EuSizeMax(s);
        }
    }
    const handleC1NaSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c1_na_size_min', s);
            setC1NaSizeMin(s);
        }
    }
    const handleC1NaSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c1_na_size_max', s);
            setC1NaSizeMax(s);
        }
    }
    const handleC2NaSizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c2_na_size_min', s);
            setC2NaSizeMin(s);
        }
    }
    const handleC2NaSizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_c2_na_size_max', s);
            setC2NaSizeMax(s);
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
    const handleODetailedChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_o_detailed', s);
            setOSizesDetailed(s);
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
    const handleO1SizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_o1_size_min', s);
            setO1SizeMin(s);
        }
    }
    const handleO1SizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_o1_size_max', s);
            setO1SizeMax(s);
        }
    }
    const handleO2SizeMinChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_o2_size_min', s);
            setO2SizeMin(s);
        }
    }
    const handleO2SizeMaxChange = (s) => {
        if (!debug) {
            engine.trigger('zone_spawn_custom.set_o2_size_max', s);
            setO2SizeMax(s);
        }
    }

    if (!debug) {
        useDataUpdate(react, "zone_spawn_custom.r_sizes", setRSizes)
        useDataUpdate(react, "zone_spawn_custom.r1_eu_sizes", setR1EuSizes)
        useDataUpdate(react, "zone_spawn_custom.r2_eu_sizes", setR2EuSizes)
        useDataUpdate(react, "zone_spawn_custom.r3_eu_sizes", setR3EuSizes)
        useDataUpdate(react, "zone_spawn_custom.r4_eu_sizes", setR4EuSizes)
        useDataUpdate(react, "zone_spawn_custom.r6_eu_sizes", setR6EuSizes)
        useDataUpdate(react, "zone_spawn_custom.r1_na_sizes", setR1NaSizes)
        useDataUpdate(react, "zone_spawn_custom.r2_na_sizes", setR2NaSizes)
        useDataUpdate(react, "zone_spawn_custom.r3_na_sizes", setR3NaSizes)
        useDataUpdate(react, "zone_spawn_custom.r4_na_sizes", setR4NaSizes)
        useDataUpdate(react, "zone_spawn_custom.r6_na_sizes", setR6NaSizes)
        useDataUpdate(react, "zone_spawn_custom.r5_sizes", setR5Sizes)

        useDataUpdate(react, "zone_spawn_custom.c_sizes", setCSizes)
        useDataUpdate(react, "zone_spawn_custom.c1_eu_sizes", setC1EuSizes)
        useDataUpdate(react, "zone_spawn_custom.c2_eu_sizes", setC2EuSizes)
        useDataUpdate(react, "zone_spawn_custom.c1_na_sizes", setC1NaSizes)
        useDataUpdate(react, "zone_spawn_custom.c2_na_sizes", setC2NaSizes)

        useDataUpdate(react, "zone_spawn_custom.i_sizes", setISizes)

        useDataUpdate(react, "zone_spawn_custom.o_sizes", setOSizes)
        useDataUpdate(react, "zone_spawn_custom.o1_sizes", setO1Sizes)
        useDataUpdate(react, "zone_spawn_custom.o2_sizes", setO2Sizes)

        useDataUpdate(react, "zone_spawn_custom.r_enabled", setRSizesEnabled)
        useDataUpdate(react, "zone_spawn_custom.r_detailed", setRSizesDetailed)
        useDataUpdate(react, "zone_spawn_custom.r_size_min", setRSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r_size_max", setRSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r1_eu_size_min", setR1EuSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r1_eu_size_max", setR1EuSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r2_eu_size_min", setR2EuSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r2_eu_size_max", setR2EuSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r3_eu_size_min", setR3EuSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r3_eu_size_max", setR3EuSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r4_eu_size_min", setR4EuSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r4_eu_size_max", setR4EuSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r6_eu_size_min", setR6EuSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r6_eu_size_max", setR6EuSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r1_na_size_min", setR1NaSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r1_na_size_max", setR1NaSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r2_na_size_min", setR2NaSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r2_na_size_max", setR2NaSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r3_na_size_min", setR3NaSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r3_na_size_max", setR3NaSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r4_na_size_min", setR4NaSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r4_na_size_max", setR4NaSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r6_na_size_min", setR6NaSizeMin)
        useDataUpdate(react, "zone_spawn_custom.r6_na_size_max", setR6NaSizeMax)
        useDataUpdate(react, "zone_spawn_custom.r5_size_min", setR5SizeMin)
        useDataUpdate(react, "zone_spawn_custom.r5_size_max", setR5SizeMax)

        useDataUpdate(react, "zone_spawn_custom.c_enabled", setCSizesEnabled)
        useDataUpdate(react, "zone_spawn_custom.c_detailed", setCSizesDetailed)
        useDataUpdate(react, "zone_spawn_custom.c_size_min", setCSizeMin)
        useDataUpdate(react, "zone_spawn_custom.c_size_max", setCSizeMax)
        useDataUpdate(react, "zone_spawn_custom.c1_eu_size_min", setC1EuSizeMin)
        useDataUpdate(react, "zone_spawn_custom.c1_eu_size_max", setC1EuSizeMax)
        useDataUpdate(react, "zone_spawn_custom.c2_eu_size_min", setC2EuSizeMin)
        useDataUpdate(react, "zone_spawn_custom.c2_eu_size_max", setC2EuSizeMax)
        useDataUpdate(react, "zone_spawn_custom.c1_na_size_min", setC1NaSizeMin)
        useDataUpdate(react, "zone_spawn_custom.c1_na_size_max", setC1NaSizeMax)
        useDataUpdate(react, "zone_spawn_custom.c2_na_size_min", setC2NaSizeMin)
        useDataUpdate(react, "zone_spawn_custom.c2_na_size_max", setC2NaSizeMax)

        useDataUpdate(react, "zone_spawn_custom.i_enabled", setISizesEnabled)
        useDataUpdate(react, "zone_spawn_custom.i_size_min", setISizeMin)
        useDataUpdate(react, "zone_spawn_custom.i_size_max", setISizeMax)

        useDataUpdate(react, "zone_spawn_custom.o_enabled", setOSizesEnabled)
        useDataUpdate(react, "zone_spawn_custom.o_detailed", setOSizesDetailed)
        useDataUpdate(react, "zone_spawn_custom.o_size_min", setOSizeMin)
        useDataUpdate(react, "zone_spawn_custom.o_size_max", setOSizeMax)
        useDataUpdate(react, "zone_spawn_custom.o1_size_min", setO1SizeMin)
        useDataUpdate(react, "zone_spawn_custom.o1_size_max", setO1SizeMax)
        useDataUpdate(react, "zone_spawn_custom.o2_size_min", setO2SizeMin)
        useDataUpdate(react, "zone_spawn_custom.o2_size_max", setO2SizeMax)

        useDataUpdate(react, "zone_spawn_custom.residential", setRStr)
        useDataUpdate(react, "zone_spawn_custom.commercial", setCStr)
        useDataUpdate(react, "zone_spawn_custom.industrial", setIStr)
        useDataUpdate(react, "zone_spawn_custom.office", setOStr)
    }

    const rData = {
        isEU: false,
        data: [
            { minSizes: rSizes.slice(0, rSizes.indexOf(rSizeMax) + 1), maxSizes: rSizes.slice(rSizes.indexOf(rSizeMin) - rSizes.length), min: rSizeMin, max: rSizeMax, onMinChnaged: handleRSizeMinChange, onMaxChanged: handleRSizeMaxChange, icon: 'Media/Game/Icons/ZoneResidential.svg' },
        ]
    };
    const rEuData = {
        isEU: true,
        data: [
            { minSizes: r1EuSizes.slice(0, r1EuSizes.indexOf(r1EuSizeMax) + 1), maxSizes: r1EuSizes.slice(r1EuSizes.indexOf(r1EuSizeMin) - r1EuSizes.length), min: r1EuSizeMin, max: r1EuSizeMax, onMinChnaged: handleR1EuSizeMinChange, onMaxChanged: handleR1EuSizeMaxChange, icon: 'Media/Game/Icons/ZoneResidentialLow.svg' },
            { minSizes: r2EuSizes.slice(0, r2EuSizes.indexOf(r2EuSizeMax) + 1), maxSizes: r2EuSizes.slice(r2EuSizes.indexOf(r2EuSizeMin) - r2EuSizes.length), min: r2EuSizeMin, max: r2EuSizeMax, onMinChnaged: handleR2EuSizeMinChange, onMaxChanged: handleR2EuSizeMaxChange, icon: 'Media/Game/Icons/ZoneResidentialMediumRow.svg' },
            { minSizes: r3EuSizes.slice(0, r3EuSizes.indexOf(r3EuSizeMax) + 1), maxSizes: r3EuSizes.slice(r3EuSizes.indexOf(r3EuSizeMin) - r3EuSizes.length), min: r3EuSizeMin, max: r3EuSizeMax, onMinChnaged: handleR3EuSizeMinChange, onMaxChanged: handleR3EuSizeMaxChange, icon: 'Media/Game/Icons/ZoneResidentialMedium.svg' },
            { minSizes: r4EuSizes.slice(0, r4EuSizes.indexOf(r4EuSizeMax) + 1), maxSizes: r4EuSizes.slice(r4EuSizes.indexOf(r4EuSizeMin) - r4EuSizes.length), min: r4EuSizeMin, max: r4EuSizeMax, onMinChnaged: handleR4EuSizeMinChange, onMaxChanged: handleR4EuSizeMaxChange, icon: 'Media/Game/Icons/ZoneResidentialMixed.svg' },
            { minSizes: r5Sizes.slice(0, r5Sizes.indexOf(r5SizeMax) + 1), maxSizes: r5Sizes.slice(r5Sizes.indexOf(r5SizeMin) - r5Sizes.length), min: r5SizeMin, max: r5SizeMax, onMinChnaged: handleR5SizeMinChange, onMaxChanged: handleR5SizeMaxChange, icon: 'Media/Game/Icons/ZoneResidentialLowRent.svg' },
            { minSizes: r6EuSizes.slice(0, r6EuSizes.indexOf(r6EuSizeMax) + 1), maxSizes: r6EuSizes.slice(r6EuSizes.indexOf(r6EuSizeMin) - r6EuSizes.length), min: r6EuSizeMin, max: r6EuSizeMax, onMinChnaged: handleR6EuSizeMinChange, onMaxChanged: handleR6EuSizeMaxChange, icon: 'Media/Game/Icons/ZoneResidentialHigh.svg' },
        ]
    };
    const rNaData = {
        isEU: false,
        data: [
            { minSizes: r1NaSizes.slice(0, r1NaSizes.indexOf(r1NaSizeMax) + 1), maxSizes: r1NaSizes.slice(r1NaSizes.indexOf(r1NaSizeMin) - r1NaSizes.length), min: r1NaSizeMin, max: r1NaSizeMax, onMinChnaged: handleR1NaSizeMinChange, onMaxChanged: handleR1NaSizeMaxChange, icon: 'Media/Game/Icons/ZoneNAResidentialLow.svg', },
            { minSizes: r2NaSizes.slice(0, r2NaSizes.indexOf(r2NaSizeMax) + 1), maxSizes: r2NaSizes.slice(r2NaSizes.indexOf(r2NaSizeMin) - r2NaSizes.length), min: r2NaSizeMin, max: r2NaSizeMax, onMinChnaged: handleR2NaSizeMinChange, onMaxChanged: handleR2NaSizeMaxChange, icon: 'Media/Game/Icons/ZoneNAResidentialMediumRow.svg' },
            { minSizes: r3NaSizes.slice(0, r3NaSizes.indexOf(r3NaSizeMax) + 1), maxSizes: r3NaSizes.slice(r3NaSizes.indexOf(r3NaSizeMin) - r3NaSizes.length), min: r3NaSizeMin, max: r3NaSizeMax, onMinChnaged: handleR3NaSizeMinChange, onMaxChanged: handleR3NaSizeMaxChange, icon: 'Media/Game/Icons/ZoneNAResidentialMedium.svg' },
            { minSizes: r4NaSizes.slice(0, r4NaSizes.indexOf(r4NaSizeMax) + 1), maxSizes: r4NaSizes.slice(r4NaSizes.indexOf(r4NaSizeMin) - r4NaSizes.length), min: r4NaSizeMin, max: r4NaSizeMax, onMinChnaged: handleR4NaSizeMinChange, onMaxChanged: handleR4NaSizeMaxChange, icon: 'Media/Game/Icons/ZoneNAResidentialMixed.svg' },
            { minSizes: r5Sizes.slice(0, r5Sizes.indexOf(r5SizeMax) + 1), maxSizes: r5Sizes.slice(r5Sizes.indexOf(r5SizeMin) - r5Sizes.length), min: r5SizeMin, max: r5SizeMax, onMinChnaged: handleR5SizeMinChange, onMaxChanged: handleR5SizeMaxChange, icon: 'Media/Game/Icons/ZoneResidentialLowRent.svg' },
            { minSizes: r6NaSizes.slice(0, r6NaSizes.indexOf(r6NaSizeMax) + 1), maxSizes: r6NaSizes.slice(r6NaSizes.indexOf(r6NaSizeMin) - r6NaSizes.length), min: r6NaSizeMin, max: r6NaSizeMax, onMinChnaged: handleR6NaSizeMinChange, onMaxChanged: handleR6NaSizeMaxChange, icon: 'Media/Game/Icons/ZoneNAResidentialHigh.svg' },
        ]
    };

    const cData = {
        isEU: false,
        data: [
            { minSizes: cSizes.slice(0, cSizes.indexOf(cSizeMax) + 1), maxSizes: cSizes.slice(cSizes.indexOf(cSizeMin) - cSizes.length), min: cSizeMin, max: cSizeMax, onMinChnaged: handleCSizeMinChange, onMaxChanged: handleCSizeMaxChange, icon: 'Media/Game/Icons/ZoneCommercial.svg' },
        ]
    };
    const cEuData = {
        isEU: true,
        data: [
            { minSizes: c1EuSizes.slice(0, c1EuSizes.indexOf(c1EuSizeMax) + 1), maxSizes: c1EuSizes.slice(c1EuSizes.indexOf(c1EuSizeMin) - c1EuSizes.length), min: c1EuSizeMin, max: c1EuSizeMax, onMinChnaged: handleC1EuSizeMinChange, onMaxChanged: handleC1EuSizeMaxChange, icon: 'Media/Game/Icons/ZoneCommercialLow.svg' },
            { minSizes: c2EuSizes.slice(0, c2EuSizes.indexOf(c2EuSizeMax) + 1), maxSizes: c2EuSizes.slice(c2EuSizes.indexOf(c2EuSizeMin) - c2EuSizes.length), min: c2EuSizeMin, max: c2EuSizeMax, onMinChnaged: handleC2EuSizeMinChange, onMaxChanged: handleC2EuSizeMaxChange, icon: 'Media/Game/Icons/ZoneCommercialHigh.svg' },
        ]
    };
    const cNaData = {
        isEU: false,
        data: [
            { minSizes: c1NaSizes.slice(0, c1NaSizes.indexOf(c1NaSizeMax) + 1), maxSizes: c1NaSizes.slice(c1NaSizes.indexOf(c1NaSizeMin) - c1NaSizes.length), min: c1NaSizeMin, max: c1NaSizeMax, onMinChnaged: handleC1NaSizeMinChange, onMaxChanged: handleC1NaSizeMaxChange, icon: 'Media/Game/Icons/ZoneNACommercialLow.svg' },
            { minSizes: c2NaSizes.slice(0, c2NaSizes.indexOf(c2NaSizeMax) + 1), maxSizes: c2NaSizes.slice(c2NaSizes.indexOf(c2NaSizeMin) - c2NaSizes.length), min: c2NaSizeMin, max: c2NaSizeMax, onMinChnaged: handleC2NaSizeMinChange, onMaxChanged: handleC2NaSizeMaxChange, icon: 'Media/Game/Icons/ZoneNACommercialHigh.svg' },
        ]
    };

    const iData = {
        isEU: false,
        data: [
            { minSizes: iSizes.slice(0, iSizes.indexOf(iSizeMax) + 1), maxSizes: iSizes.slice(iSizes.indexOf(iSizeMin) - iSizes.length), min: iSizeMin, max: iSizeMax, onMinChnaged: handleISizeMinChange, onMaxChanged: handleISizeMaxChange, icon: 'Media/Game/Icons/ZoneIndustrial.svg' },
        ]
    };

    const oData = {
        isEU: false,
        data: [
            { minSizes: oSizes.slice(0, oSizes.indexOf(oSizeMax) + 1), maxSizes: oSizes.slice(oSizes.indexOf(oSizeMin) - oSizes.length), min: oSizeMin, max: oSizeMax, onMinChnaged: handleOSizeMinChange, onMaxChanged: handleOSizeMaxChange, icon: 'Media/Game/Icons/ZoneOffice.svg' },
        ]
    };
    const oDetailData = {
        isEU: false,
        data: [
            { minSizes: o1Sizes.slice(0, o1Sizes.indexOf(o1SizeMax) + 1), maxSizes: o1Sizes.slice(o1Sizes.indexOf(o1SizeMin) - o1Sizes.length), min: o1SizeMin, max: o1SizeMax, onMinChnaged: handleO1SizeMinChange, onMaxChanged: handleO1SizeMaxChange, icon: 'Media/Game/Icons/ZoneOfficeLow.svg' },
            { minSizes: o2Sizes.slice(0, o2Sizes.indexOf(o2SizeMax) + 1), maxSizes: o2Sizes.slice(o2Sizes.indexOf(o2SizeMin) - o2Sizes.length), min: o2SizeMin, max: o2SizeMax, onMinChnaged: handleO2SizeMinChange, onMaxChanged: handleO2SizeMaxChange, icon: 'Media/Game/Icons/ZoneOfficeHigh.svg' },
        ]
    };

    return <div>
        <$SettingPage react={react} title={rStr} enabled={rSizesEnabled} normalData={rData} hasDetail='true' detailed={rSizesDetailed} hasTheme='true' detailDatas={[rEuData, rNaData]} onEnabledChanged={handleREnabledChange} onDetailedChanged={handleRDetailedChange}></$SettingPage>
        <$SettingPage react={react} title={cStr} enabled={cSizesEnabled} normalData={cData} hasDetail='true' detailed={cSizesDetailed} hasTheme='true' detailDatas={[cEuData, cNaData]} onEnabledChanged={handleCEnabledChange} onDetailedChanged={handleCDetailedChange}></$SettingPage>
        <$SettingPage react={react} title={iStr} enabled={iSizesEnabled} normalData={iData} onEnabledChanged={handleIEnabledChange}></$SettingPage>
        <$SettingPage react={react} title={oStr} enabled={oSizesEnabled} normalData={oData} hasDetail='true' detailed={oSizesDetailed} detailDatas={[oDetailData]} onEnabledChanged={handleOEnabledChange} onDetailedChanged={handleODetailedChange}></$SettingPage>
    </div>
}

export const $App = ({ react, debug = false }) => {
    const [title, setTitle] = react.useState('Zone Spawn Custom')
    useDataUpdate(react, 'zone_spawn_custom.title', setTitle)

    return <$Panel title={title } react={react} id={panelID} maxHeight='585rem'>
        <$DataPage react={react} debug={debug}></$DataPage>
    </$Panel>
}