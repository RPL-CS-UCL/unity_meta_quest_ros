// Auto-generated. Do not edit!

// (in-package unity_meta_quest_msgs.srv)


"use strict";

const _serializer = _ros_msg_utils.Serialize;
const _arraySerializer = _serializer.Array;
const _deserializer = _ros_msg_utils.Deserialize;
const _arrayDeserializer = _deserializer.Array;
const _finder = _ros_msg_utils.Find;
const _getByteLength = _ros_msg_utils.getByteLength;
let PosRot = require('../msg/PosRot.js');

//-----------------------------------------------------------


//-----------------------------------------------------------

class PositionServiceRequest {
  constructor(initObj={}) {
    if (initObj === null) {
      // initObj === null is a special case for deserialization where we don't initialize fields
      this.input = null;
    }
    else {
      if (initObj.hasOwnProperty('input')) {
        this.input = initObj.input
      }
      else {
        this.input = new PosRot();
      }
    }
  }

  static serialize(obj, buffer, bufferOffset) {
    // Serializes a message object of type PositionServiceRequest
    // Serialize message field [input]
    bufferOffset = PosRot.serialize(obj.input, buffer, bufferOffset);
    return bufferOffset;
  }

  static deserialize(buffer, bufferOffset=[0]) {
    //deserializes a message object of type PositionServiceRequest
    let len;
    let data = new PositionServiceRequest(null);
    // Deserialize message field [input]
    data.input = PosRot.deserialize(buffer, bufferOffset);
    return data;
  }

  static getMessageSize(object) {
    let length = 0;
    length += PosRot.getMessageSize(object.input);
    return length;
  }

  static datatype() {
    // Returns string type for a service object
    return 'unity_meta_quest_msgs/PositionServiceRequest';
  }

  static md5sum() {
    //Returns md5sum for a message object
    return '8e1dd27b40d577e01a5f9128d82cf0d0';
  }

  static messageDefinition() {
    // Returns full string definition for message
    return `
    PosRot input
    
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
    const resolved = new PositionServiceRequest(null);
    if (msg.input !== undefined) {
      resolved.input = PosRot.Resolve(msg.input)
    }
    else {
      resolved.input = new PosRot()
    }

    return resolved;
    }
};

class PositionServiceResponse {
  constructor(initObj={}) {
    if (initObj === null) {
      // initObj === null is a special case for deserialization where we don't initialize fields
      this.output = null;
    }
    else {
      if (initObj.hasOwnProperty('output')) {
        this.output = initObj.output
      }
      else {
        this.output = new PosRot();
      }
    }
  }

  static serialize(obj, buffer, bufferOffset) {
    // Serializes a message object of type PositionServiceResponse
    // Serialize message field [output]
    bufferOffset = PosRot.serialize(obj.output, buffer, bufferOffset);
    return bufferOffset;
  }

  static deserialize(buffer, bufferOffset=[0]) {
    //deserializes a message object of type PositionServiceResponse
    let len;
    let data = new PositionServiceResponse(null);
    // Deserialize message field [output]
    data.output = PosRot.deserialize(buffer, bufferOffset);
    return data;
  }

  static getMessageSize(object) {
    let length = 0;
    length += PosRot.getMessageSize(object.output);
    return length;
  }

  static datatype() {
    // Returns string type for a service object
    return 'unity_meta_quest_msgs/PositionServiceResponse';
  }

  static md5sum() {
    //Returns md5sum for a message object
    return 'aabb3c03575b6efd312478691e8e83dc';
  }

  static messageDefinition() {
    // Returns full string definition for message
    return `
    PosRot output
    
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
    const resolved = new PositionServiceResponse(null);
    if (msg.output !== undefined) {
      resolved.output = PosRot.Resolve(msg.output)
    }
    else {
      resolved.output = new PosRot()
    }

    return resolved;
    }
};

module.exports = {
  Request: PositionServiceRequest,
  Response: PositionServiceResponse,
  md5sum() { return '74d162dfa312ad5f1087485ee56d06ee'; },
  datatype() { return 'unity_meta_quest_msgs/PositionService'; }
};
