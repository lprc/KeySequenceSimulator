# KeySequenceSimulator
KeySequenceSimulator lets you simulate key and mouse events. It's in an experimental state so there definitely might be some issues.

Dependencies:
* [Avalonia](https://github.com/AvaloniaUI/Avalonia) as GUI library
* [InputSimulatorPlus](https://github.com/TChatzigiannakis/InputSimulatorPlus) for simulating input
* [globalmousekeyhook](https://github.com/gmamaladze/globalmousekeyhook) for catching key events

Because of the last two dependencies the project is restricted to windows only. Anyways the backend might be developed for other OSes by implementing the interfaces `IGlobalInput` and `IActionSimulator`.

![grafik](https://user-images.githubusercontent.com/13603403/95175398-b7de2580-07bb-11eb-9097-c7a35fa0a264.png)
