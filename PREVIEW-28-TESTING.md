# Preview 28 test checklist

- Console: the checkbox and label for Automatic scrolling are optically centered with the toolbar controls in Light and Dark themes.
- Trigger page: its outer content bounds match the other main pages; clipped trigger values expose their full value through a tooltip.
- Scaling: inspect the main pages at 100%, 125% and 150%; scrolling remains available as a fallback and controls remain reachable.
- Disabled buttons: Twitch chat connection, Nanoleaf connection testing and trigger editing explain their prerequisite in a tooltip.
- Keyboard: trigger search receives focus on opening; Escape returns from trigger/info/management content.
- Settings recovery: invalid settings are replaced safely and preserved as an `.invalid-*.bak` file.
- HypeRate: missing settings and malformed messages are ignored and logged rather than crashing.
- Trigger queue: count updates are dispatched asynchronously and reset drains the active queue without stranding its handler.
