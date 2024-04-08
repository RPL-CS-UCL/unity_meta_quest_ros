# ControllerState.msg
# This message holds the state of the buttons and thumbstick on the left XR controller.

# Thumbstick values are usually represented as a 2D vector with x and y components.
float32 thumbstick_x
float32 thumbstick_y

# Buttons can be represented as booleans, where true means pressed, and false means not pressed.
bool trigger_pressed
bool grip_pressed
bool primary_button_pressed
bool secondary_button_pressed