// Auto-generated. Do not edit!

// (in-package unity_meta_quest_msgs.msg)


"use strict";

const _serializer = _ros_msg_utils.Serialize;
const _arraySerializer = _serializer.Array;
const _deserializer = _ros_msg_utils.Deserialize;
const _arrayDeserializer = _deserializer.Array;
const _finder = _ros_msg_utils.Find;
const _getByteLength = _ros_msg_utils.getByteLength;
let PosRot = require('./PosRot.js');

//-----------------------------------------------------------

class PosRotList {
  constructor(initObj={}) {
    if (initObj === null) {
      // initObj === null is a special case for deserialization where we don't initialize fields
      this.devices = null;
    }
    else {
      if (initObj.hasOwnProperty('devices')) {
        this.devices = initObj.devices
      }
      else {
        this.devices = [];
      }
    }
  }

  static serialize(obj, buffer, bufferOffset) {
    // Serializes a message object of type PosRotList
    // Serialize message field [devices]
    // Serialize the length for message field [devices]
    bufferOffset = _serializer.uint32(obj.devices.length, buffer, bufferOffset);
    obj.devices.forEach((val) => {
      bufferOffset = PosRot.serialize(val, buffer, bufferOffset);
    });
    return bufferOffset;
  }

  static deserialize(buffer, bufferOffset=[0]) {
    //deserializes a message object of type PosRotList
    let len;
    let data = new PosRotList(null);
    // Deserialize message field [devices]
    // Deserialize array length for message field [devices]
    len = _deserializer.uint32(buffer, bufferOffset);
    data.devices = new Array(len);
    for (let i = 0; i < len; ++i) {
      data.devices[i] = PosRot.deserialize(buffer, bufferOffset)
    }
    return data;
  }

  static getMessageSize(object) {
    let length = 0;
    object.devices.forEach((val) => {
      length += PosRot.getMessageSize(val);
    });
    return length + 4;
  }

  static datatype() {
    // Returns string type for a message object
    return 'unity_meta_quest_msgs/PosRotList';
  }

  static md5sum() {
    //Returns md5sum for a message object
    return '439e0451c819cb409f987681338dd04c';
  }

  static messageDefinition() {
    // Returns full string definition for message
    return `
    PosRot[] devices
    ================================================================================
    MSG: unity_meta_quest_msgs/PosRot
    float32 pos_x
    float32 pos_y
    float32 pos_z
    float32 rot_x
    float32 rot_y
    float32 rot_z
    float32 rot_w
    string name
    `;
  }

  static Resolve(msg) {
    // deep-construct a valid message object instance of whatever was passed in
    if (typeof msg !== 'object' || msg === null) {
      msg = {};
    }
    const resolved = new PosRotList(null);
    if (msg.devices !== undefined) {
      resolved.devices = new Array(msg.devices.length);
      for (let i = 0; i < resolved.devices.length; ++i) {
        resolved.devices[i] = PosRot.Resolve(msg.devices[i]);
      }
    }
    else {
      resolved.devices = []
    }

    return resolved;
    }
};

module.exports = PosRotList;
