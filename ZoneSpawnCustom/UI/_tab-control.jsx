import React from 'react'

const $TabControl = ({ react, tabs, style }) => {
    const [activeTab, setActiveTab] = react.useState(tabs.length > 0 ? tabs[0].name : '');

    return (
        <div style={style} >
            <div className="panel_YqS" style={{ marginLeft: 'auto', marginRight: 'auto', width: '100%' }}>
                <div className="tab-bar_oPw" style={{ padding: '0' }}>
                    {tabs.map(tab => (
                        <div key={tab.name} className={`tab_Hrb ${activeTab === tab.name ? 'selected' : ''}`} style={{ flex: '1' }} onClick={() => setActiveTab(tab.name)} style={{ flex: '1', borderRadius: '0' }}>
                            {tab.icon ? <img src={tab.icon} alt='' style={{ maxWidth: '32rem' }}></img> : null}
                            {tab.iconOnly ? null : <div> {tab.name} </div>}
                        </div>
                    ))}
                </div>
                <div>
                    {tabs.map(tab => (
                        <div
                            key={tab.name}
                            style={{ display: activeTab === tab.name ? 'flex' : 'none', flexDirection: 'row', paddingTop: '10rem' }}
                        >
                            {tab.content}
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default $TabControl;
