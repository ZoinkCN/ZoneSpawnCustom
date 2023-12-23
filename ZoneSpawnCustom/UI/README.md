# HookUI Framework

> JavaScript library that helps you integrate with Cities: Skylines 2 Game UI. Helps with both styling and communication with your C# mods.

- [List of Components + API](./components.md)

## Usage for UI authors

- Install the framework `npm install --save captain-of-coit/hookui-framework`
- Import what you need `import {$Panel} from 'hookui-framework'`
- Use it as a React component:

```jsx
<$Panel title="My first panel">
    <h1>This is inside the panel</h1>
</$Panel>
```

Use `useDataUpdate` for easier subscriptions to data coming from the game engine:

```jsx
import {useDataUpdate} from 'hookui-framework'

const $MyCoolMod = ({ react }) => {
    const [seconds, setSeconds] = react.useState(0);

    useDataUpdate(react, "myowncoolmod_namespace.seconds_passed", (data) => {
        setSeconds(data)
    })

    return <div>
        It's been {seconds} seconds since the mod first loaded!
    </div>
}
```

## Setup for Development Environment

- Copy `Cities2_Data\StreamingAssets\~UI~\GameUI\index.css` to `dev-env/index.css`
- Copy `Cities2_Data\StreamingAssets\~UI~\GameUI\Media` to `dev-env/Media`
- Run `npm install`
- Run `node build.mjs`
- Navigate to `localhost:8000`
- Select a component on the left to inspect it