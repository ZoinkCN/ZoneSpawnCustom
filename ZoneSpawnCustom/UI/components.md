# Components

- [$Panel](#panel)
- [$Meter](#meter)
- [$Field](#field)

### `$Panel`

<img src="https://github.com/Captain-Of-Coit/hookui-framework/assets/126259864/ee2eed79-a506-4784-b4d5-a84b1b507e2f" width="400">

```jsx
<$Panel title="Hello" react={React}>
    <h3>Waddup world</h3>
</$Panel>
```

---

### `$Meter`

<img src="https://github.com/Captain-Of-Coit/hookui-framework/assets/126259864/af0ee89a-a724-4523-9675-a19d50023a44" width="400">

```jsx
<$Meter label="Electricity Availability" value={75} gradient="maxGood" />

<$Meter label="Fire Hazard" value={25} gradient="minGood" />
```

---

### `$Field`

<img src="https://github.com/Captain-Of-Coit/hookui-framework/assets/126259864/4584868d-a659-49cb-82c3-661744fe1384" width="400">


```jsx
const $InteractiveField = ({react}) => {
    const [checked, setChecked] = react.useState(false)
    return <$Field label="Toggle Me" checked={checked} onToggle={setChecked} />
}
```
