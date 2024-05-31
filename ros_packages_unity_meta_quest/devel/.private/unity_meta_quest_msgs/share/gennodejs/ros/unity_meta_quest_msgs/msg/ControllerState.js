// Auto-generated. Do not edit!

// (in-package unity_meta_quest_msgs.msg)


"use strict";

const _serializer = _ros_msg_utils.Serialize;
const _arraySerializer = _serializer.Array;
const _deserializer = _ros_msg_utils.Deserialize;
const _arrayDeserializer = _deserializer.Array;
const _finder = _ros_msg_utils.Find;
const _getByteLength = _ros_msg_utils.getByteLength;

//-----------------------------------------------------------

class ControllerState {
  constructor(initObj={}) {
    if (initObj === null) {
      // initObj === null is a special case for deserialization where we don't initialize fields
      this.thumbstick_x = null;
      this.thumbstick_y = null;
      this.trigger_pressed = null;
      this.grip_pressed = null;
      this.primary_button_pressed = null;
      this.secondary_button_pressed = null;
    }
    else {
      if (initObj.hasOwnProperty('thumbstick_x')) {
        this.thumbstick_x = initObj.thumbstick_x
      }
      else {
        this.thumbstick_x = 0.0;
      }
      if (initObj.hasOwnProperty('thumbstick_y')) {
        this.thumbstick_y = initObj.thumbstick_y
      }
      else {
        this.thumbstick_y = 0.0;
      }
      if (initObj.hasOwnProperty('trigger_pressed')) {
        this.trigger_pressed = initObj.trigger_pressed
      }
      else {
        this.trigger_pressed = false;
      }
      if (initObj.hasOwnProperty('grip_pressed')) {
        this.grip_pressed = initObj.grip_pressed
      }
      else {
        this.grip_pressed = false;
      }
      if (initObj.hasOwnProperty('primary_button_pressed')) {
        this.primary_button_pressed = initObj.primary_button_pressed
      }
      else {
        this.primary_button_pressed = false;
      }
      if (initObj.hasOwnProperty('secondary_button_pressed')) {
        this.secondary_button_pressed = initObj.secondary_button_pressed
      }
      else {
        this.secondary_button_pressed = false;
      }
    }
  }

  static serialize(obj, buffer, bufferOffset) {
    // Serializes a message object of type ControllerState
    // Serialize message field [thumbstick_x]
    bufferOffset = _serializer.float32(obj.thumbstick_x, buffer, bufferOffset);
    // Serialize message field [thumbstick_y]
    bufferOffset = _serializer.float32(obj.thumbstick_y, buffer, bufferOffset);
    // Serialize message field [trigger_pressed]
    bufferOffset = _serializer.bool(obj.trigger_pressed, buffer, bufferOffset);
    // Serialize message field [grip_pressed]
    bufferOffset = _serializer.bool(obj.grip_pressed, buffer, bufferOffset);
    // Serialize message field [primary_button_pressed]
    bufferOffset = _serializer.bool(obj.primary_button_pressed, buffer, bufferOffset);
    // Serialize message field [secondary_button_pressed]
    bufferOffset = _serializer.bool(obj.secondary_button_pressed, buffer, bufferOffset);
    return bufferOffset;
  }

  static deserialize(buffer, bufferOffset=[0]) {
    //deserializes a message object of type ControllerState
    let len;
    let data = new ControllerState(null);
    // Deserialize message field [thumbstick_x]
    data.thumbstick_x = _deserializer.float32(buffer, bufferOffset);
    // Deserialize message field [thumbstick_y]
    data.thumbstick_y = _deserializer.float32(buffer, bufferOffset);
    // Deserialize message field [trigger_pressed]
    data.trigger_pressed = _deserializer.bool(buffer, bufferOffset);
    // Deserialize message field [grip_pressed]
    data.grip_pressed = _deserializer.bool(buffer, bufferOffset);
    // Deserialize message field [primary_button_pressed]
    data.primary_button_pressed = _deserializer.bool(buffer, bufferOffset);
    // Deserialize message field [secondary_button_pressed]
    data.secondary_button_pressed = _deserializer.bool(buffer, bufferOffset);
    return data;
  }

  static getMessageSize(object) {
    return 12;
  }

  static datatype() {
    // Returns string type for a message object
    return 'unity_meta_quest_msgs/ControllerState';
  }

  static md5sum() {
    //Returns md5sum for a message object
    return '13c77d47a3ccdc5ae9cfb2e89658444a';
  }

  static messageDefinition() {
    // Returns full string definition for message
    return `
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
    `;
  }

  static Resolve(msg) {
    // deep-construct a valid message object instance of whatever was passed in
    if (typeof msg !== 'object' || msg === null) {
      msg = {};
    }
    const resolved = new ControllerState(null);
    if (msg.thumbstick_x !== undefined) {
      resolved.thumbstick_x = msg.thumbstick_x;
    }
    else {
      resolved.thumbstick_x = 0.0
    }

    if (msg.thumbstick_y !== undefined) {
      resolved.thumbstick_y = msg.thumbstick_y;
    }
    else {
      resolved.thumbstick_y = 0.0
    }

    if (msg.trigger_pressed !== undefined) {
      resolved.trigger_pressed = msg.trigger_pressed;
    }
    else {
      resolved.trigger_pressed = false
    }

    if (msg.grip_pressed !== undefined) {
      resolved.grip_pressed = msg.grip_pressed;
    }
    else {
      resolved.grip_pressed = false
    }

    if (msg.primary_button_pressed !== undefined) {
      resolved.primary_button_pressed = msg.primary_button_pressed;
    }
    else {
      resolved.primary_button_pressed = false
    }

    if (msg.secondary_button_pressed !== undefined) {
      resolved.secondary_button_pressed = msg.secondary_button_pressed;
    }
    else {
      resolved.secondary_button_pressed = false
    }

    return resolved;
    }
};

module.exports = ControllerState;
