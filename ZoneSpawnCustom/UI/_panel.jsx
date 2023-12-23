import React from 'react';

export const $Panel = ({ title, children, react, id, maxHeight='600rem' }) => {
    const panelStyle = { position: 'absolute', maxHeight: maxHeight };
    const [position, setPosition] = react.useState({ top: 100, left: 10 });
    const [dragging, setDragging] = react.useState(false);
    const [rel, setRel] = react.useState({ x: 0, y: 0 }); // Position relative to the cursor
    const [topValue, setTopValue] = react.useState(0);

    const onMouseDown = (e) => {
        if (e.button !== 0) return; // Only left mouse button
        const panelElement = e.target.closest('.panel_YqS');

        // Calculate the initial relative position
        const rect = panelElement.getBoundingClientRect();
        setRel({
            x: e.clientX - rect.left,
            y: e.clientY - rect.top,
        });

        setDragging(true);
        e.stopPropagation();
        e.preventDefault();
    }

    const onMouseUp = (e) => {
        setDragging(false);
        // Remove window event listeners when the mouse is released
        window.removeEventListener('mousemove', onMouseMove);
        window.removeEventListener('mouseup', onMouseUp);
    }

    const onMouseMove = (e) => {
        if (!dragging) return;

        setPosition({
            top: e.clientY - rel.y,
            left: e.clientX - rel.x,
        });
        e.stopPropagation();
        e.preventDefault();
    }

    const onClose = () => {
        const data = {
            type: "toggle_visibility", id: id
        };
        const event = new CustomEvent('hookui', { detail: data });
        window.dispatchEvent(event);
    }

    const draggableStyle = {
        ...panelStyle,
        top: position.top + 'px',
        left: position.left + 'px',
    }

    const handleScroll = (event) => {
        setTopValue(event.target.scrollTop);
    }
    const closeStyle = { maskImage: 'url(Media/Glyphs/Close.svg)' };

    react.useEffect(() => {
        if (dragging) {
            // Attach event listeners to window
            window.addEventListener('mousemove', onMouseMove);
            window.addEventListener('mouseup', onMouseUp);
        }

        return () => {
            // Clean up event listeners when the component unmounts or dragging is finished
            window.removeEventListener('mousemove', onMouseMove);
            window.removeEventListener('mouseup', onMouseUp);
        };
    }, [dragging]); // Only re-run the effect if dragging state changes

    return (
        <div className="panel_YqS active-infoview-panel_aTq" style={draggableStyle}>
            <div className="header_H_U header_Bpo child-opacity-transition_nkS" onMouseDown={onMouseDown}>
                <div className="title-bar_PF4 title_Hfc">
                    <div className="title_SVH title_zQN">{title}</div>
                    <button className="button_bvQ button_bvQ close-button_wKK" onClick={onClose}>
                        <div className="tinted-icon_iKo icon_PhD" style={closeStyle}></div>
                    </button>
                </div>
            </div>
            <div className="content_XD5 content_AD7 child-opacity-transition_nkS content_BIL"
                style={{ overflowY: 'scroll', flexDirection: 'column' }}>
                <div className="section_sop section_gUk statistics-menu_y86" style={{ width: '100%' }}>
                    <div className="content_flM content_owQ first_l25 last_ZNw">
                        <div className="scrollable_DXr y_SMM track-visible-y_RCA scrollable_By7">
                            <div className="content_gqa" onScroll={handleScroll}>
                                <div className="content_Q1O">
                                    {children}
                                </div>
                                <div className="bottom-padding_JS3"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
