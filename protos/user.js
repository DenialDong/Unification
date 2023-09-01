/*eslint-disable block-scoped-var, id-length, no-control-regex, no-magic-numbers, no-prototype-builtins, no-redeclare, no-shadow, no-var, sort-vars*/
(function($protobuf) {
    "use strict";

    // Common aliases
    var $Reader = $protobuf.Reader, $Writer = $protobuf.Writer, $util = $protobuf.util;
    
    // Exported root namespace
    var $root = $protobuf.roots["default"] || ($protobuf.roots["default"] = {});
    
    $root.kop = (function() {
    
        /**
         * Namespace kop.
         * @exports kop
         * @namespace
         */
        var kop = {};
    
        kop.Item = (function() {
    
            /**
             * Properties of an Item.
             * @memberof kop
             * @interface IItem
             * @property {number|null} [ConfId] Item ConfId
             * @property {number|null} [Count] Item Count
             * @property {number|null} [Total] Item Total
             * @property {number|Long|null} [Id] Item Id
             */
    
            /**
             * Constructs a new Item.
             * @memberof kop
             * @classdesc Represents an Item.
             * @implements IItem
             * @constructor
             * @param {kop.IItem=} [properties] Properties to set
             */
            function Item(properties) {
                if (properties)
                    for (var keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                        if (properties[keys[i]] != null)
                            this[keys[i]] = properties[keys[i]];
            }
    
            /**
             * Item ConfId.
             * @member {number} ConfId
             * @memberof kop.Item
             * @instance
             */
            Item.prototype.ConfId = 0;
    
            /**
             * Item Count.
             * @member {number} Count
             * @memberof kop.Item
             * @instance
             */
            Item.prototype.Count = 0;
    
            /**
             * Item Total.
             * @member {number} Total
             * @memberof kop.Item
             * @instance
             */
            Item.prototype.Total = 0;
    
            /**
             * Item Id.
             * @member {number|Long} Id
             * @memberof kop.Item
             * @instance
             */
            Item.prototype.Id = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
    
            /**
             * Creates a new Item instance using the specified properties.
             * @function create
             * @memberof kop.Item
             * @static
             * @param {kop.IItem=} [properties] Properties to set
             * @returns {kop.Item} Item instance
             */
            Item.create = function create(properties) {
                return new Item(properties);
            };
    
            /**
             * Encodes the specified Item message. Does not implicitly {@link kop.Item.verify|verify} messages.
             * @function encode
             * @memberof kop.Item
             * @static
             * @param {kop.IItem} message Item message or plain object to encode
             * @param {$protobuf.Writer} [writer] Writer to encode to
             * @returns {$protobuf.Writer} Writer
             */
            Item.encode = function encode(message, writer) {
                if (!writer)
                    writer = $Writer.create();
                if (message.ConfId != null && Object.hasOwnProperty.call(message, "ConfId"))
                    writer.uint32(/* id 1, wireType 0 =*/8).int32(message.ConfId);
                if (message.Count != null && Object.hasOwnProperty.call(message, "Count"))
                    writer.uint32(/* id 2, wireType 0 =*/16).int32(message.Count);
                if (message.Total != null && Object.hasOwnProperty.call(message, "Total"))
                    writer.uint32(/* id 3, wireType 0 =*/24).int32(message.Total);
                if (message.Id != null && Object.hasOwnProperty.call(message, "Id"))
                    writer.uint32(/* id 4, wireType 0 =*/32).int64(message.Id);
                return writer;
            };
    
            /**
             * Encodes the specified Item message, length delimited. Does not implicitly {@link kop.Item.verify|verify} messages.
             * @function encodeDelimited
             * @memberof kop.Item
             * @static
             * @param {kop.IItem} message Item message or plain object to encode
             * @param {$protobuf.Writer} [writer] Writer to encode to
             * @returns {$protobuf.Writer} Writer
             */
            Item.encodeDelimited = function encodeDelimited(message, writer) {
                return this.encode(message, writer).ldelim();
            };
    
            /**
             * Decodes an Item message from the specified reader or buffer.
             * @function decode
             * @memberof kop.Item
             * @static
             * @param {$protobuf.Reader|Uint8Array} reader Reader or buffer to decode from
             * @param {number} [length] Message length if known beforehand
             * @returns {kop.Item} Item
             * @throws {Error} If the payload is not a reader or valid buffer
             * @throws {$protobuf.util.ProtocolError} If required fields are missing
             */
            Item.decode = function decode(reader, length) {
                if (!(reader instanceof $Reader))
                    reader = $Reader.create(reader);
                var end = length === undefined ? reader.len : reader.pos + length, message = new $root.kop.Item();
                while (reader.pos < end) {
                    var tag = reader.uint32();
                    switch (tag >>> 3) {
                    case 1:
                        message.ConfId = reader.int32();
                        break;
                    case 2:
                        message.Count = reader.int32();
                        break;
                    case 3:
                        message.Total = reader.int32();
                        break;
                    case 4:
                        message.Id = reader.int64();
                        break;
                    default:
                        reader.skipType(tag & 7);
                        break;
                    }
                }
                return message;
            };
    
            /**
             * Decodes an Item message from the specified reader or buffer, length delimited.
             * @function decodeDelimited
             * @memberof kop.Item
             * @static
             * @param {$protobuf.Reader|Uint8Array} reader Reader or buffer to decode from
             * @returns {kop.Item} Item
             * @throws {Error} If the payload is not a reader or valid buffer
             * @throws {$protobuf.util.ProtocolError} If required fields are missing
             */
            Item.decodeDelimited = function decodeDelimited(reader) {
                if (!(reader instanceof $Reader))
                    reader = new $Reader(reader);
                return this.decode(reader, reader.uint32());
            };
    
            /**
             * Verifies an Item message.
             * @function verify
             * @memberof kop.Item
             * @static
             * @param {Object.<string,*>} message Plain object to verify
             * @returns {string|null} `null` if valid, otherwise the reason why it is not
             */
            Item.verify = function verify(message) {
                if (typeof message !== "object" || message === null)
                    return "object expected";
                if (message.ConfId != null && message.hasOwnProperty("ConfId"))
                    if (!$util.isInteger(message.ConfId))
                        return "ConfId: integer expected";
                if (message.Count != null && message.hasOwnProperty("Count"))
                    if (!$util.isInteger(message.Count))
                        return "Count: integer expected";
                if (message.Total != null && message.hasOwnProperty("Total"))
                    if (!$util.isInteger(message.Total))
                        return "Total: integer expected";
                if (message.Id != null && message.hasOwnProperty("Id"))
                    if (!$util.isInteger(message.Id) && !(message.Id && $util.isInteger(message.Id.low) && $util.isInteger(message.Id.high)))
                        return "Id: integer|Long expected";
                return null;
            };
    
            /**
             * Creates an Item message from a plain object. Also converts values to their respective internal types.
             * @function fromObject
             * @memberof kop.Item
             * @static
             * @param {Object.<string,*>} object Plain object
             * @returns {kop.Item} Item
             */
            Item.fromObject = function fromObject(object) {
                if (object instanceof $root.kop.Item)
                    return object;
                var message = new $root.kop.Item();
                if (object.ConfId != null)
                    message.ConfId = object.ConfId | 0;
                if (object.Count != null)
                    message.Count = object.Count | 0;
                if (object.Total != null)
                    message.Total = object.Total | 0;
                if (object.Id != null)
                    if ($util.Long)
                        (message.Id = $util.Long.fromValue(object.Id)).unsigned = false;
                    else if (typeof object.Id === "string")
                        message.Id = parseInt(object.Id, 10);
                    else if (typeof object.Id === "number")
                        message.Id = object.Id;
                    else if (typeof object.Id === "object")
                        message.Id = new $util.LongBits(object.Id.low >>> 0, object.Id.high >>> 0).toNumber();
                return message;
            };
    
            /**
             * Creates a plain object from an Item message. Also converts values to other types if specified.
             * @function toObject
             * @memberof kop.Item
             * @static
             * @param {kop.Item} message Item
             * @param {$protobuf.IConversionOptions} [options] Conversion options
             * @returns {Object.<string,*>} Plain object
             */
            Item.toObject = function toObject(message, options) {
                if (!options)
                    options = {};
                var object = {};
                if (options.defaults) {
                    object.ConfId = 0;
                    object.Count = 0;
                    object.Total = 0;
                    if ($util.Long) {
                        var long = new $util.Long(0, 0, false);
                        object.Id = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                    } else
                        object.Id = options.longs === String ? "0" : 0;
                }
                if (message.ConfId != null && message.hasOwnProperty("ConfId"))
                    object.ConfId = message.ConfId;
                if (message.Count != null && message.hasOwnProperty("Count"))
                    object.Count = message.Count;
                if (message.Total != null && message.hasOwnProperty("Total"))
                    object.Total = message.Total;
                if (message.Id != null && message.hasOwnProperty("Id"))
                    if (typeof message.Id === "number")
                        object.Id = options.longs === String ? String(message.Id) : message.Id;
                    else
                        object.Id = options.longs === String ? $util.Long.prototype.toString.call(message.Id) : options.longs === Number ? new $util.LongBits(message.Id.low >>> 0, message.Id.high >>> 0).toNumber() : message.Id;
                return object;
            };
    
            /**
             * Converts this Item to JSON.
             * @function toJSON
             * @memberof kop.Item
             * @instance
             * @returns {Object.<string,*>} JSON object
             */
            Item.prototype.toJSON = function toJSON() {
                return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
            };
    
            return Item;
        })();
    
        kop.User = (function() {
    
            /**
             * Properties of a User.
             * @memberof kop
             * @interface IUser
             * @property {number|Long|null} [id] User id
             * @property {string|null} [name] User name
             */
    
            /**
             * Constructs a new User.
             * @memberof kop
             * @classdesc Represents a User.
             * @implements IUser
             * @constructor
             * @param {kop.IUser=} [properties] Properties to set
             */
            function User(properties) {
                if (properties)
                    for (var keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                        if (properties[keys[i]] != null)
                            this[keys[i]] = properties[keys[i]];
            }
    
            /**
             * User id.
             * @member {number|Long} id
             * @memberof kop.User
             * @instance
             */
            User.prototype.id = $util.Long ? $util.Long.fromBits(0,0,false) : 0;
    
            /**
             * User name.
             * @member {string} name
             * @memberof kop.User
             * @instance
             */
            User.prototype.name = "";
    
            /**
             * Creates a new User instance using the specified properties.
             * @function create
             * @memberof kop.User
             * @static
             * @param {kop.IUser=} [properties] Properties to set
             * @returns {kop.User} User instance
             */
            User.create = function create(properties) {
                return new User(properties);
            };
    
            /**
             * Encodes the specified User message. Does not implicitly {@link kop.User.verify|verify} messages.
             * @function encode
             * @memberof kop.User
             * @static
             * @param {kop.IUser} message User message or plain object to encode
             * @param {$protobuf.Writer} [writer] Writer to encode to
             * @returns {$protobuf.Writer} Writer
             */
            User.encode = function encode(message, writer) {
                if (!writer)
                    writer = $Writer.create();
                if (message.id != null && Object.hasOwnProperty.call(message, "id"))
                    writer.uint32(/* id 1, wireType 0 =*/8).int64(message.id);
                if (message.name != null && Object.hasOwnProperty.call(message, "name"))
                    writer.uint32(/* id 2, wireType 2 =*/18).string(message.name);
                return writer;
            };
    
            /**
             * Encodes the specified User message, length delimited. Does not implicitly {@link kop.User.verify|verify} messages.
             * @function encodeDelimited
             * @memberof kop.User
             * @static
             * @param {kop.IUser} message User message or plain object to encode
             * @param {$protobuf.Writer} [writer] Writer to encode to
             * @returns {$protobuf.Writer} Writer
             */
            User.encodeDelimited = function encodeDelimited(message, writer) {
                return this.encode(message, writer).ldelim();
            };
    
            /**
             * Decodes a User message from the specified reader or buffer.
             * @function decode
             * @memberof kop.User
             * @static
             * @param {$protobuf.Reader|Uint8Array} reader Reader or buffer to decode from
             * @param {number} [length] Message length if known beforehand
             * @returns {kop.User} User
             * @throws {Error} If the payload is not a reader or valid buffer
             * @throws {$protobuf.util.ProtocolError} If required fields are missing
             */
            User.decode = function decode(reader, length) {
                if (!(reader instanceof $Reader))
                    reader = $Reader.create(reader);
                var end = length === undefined ? reader.len : reader.pos + length, message = new $root.kop.User();
                while (reader.pos < end) {
                    var tag = reader.uint32();
                    switch (tag >>> 3) {
                    case 1:
                        message.id = reader.int64();
                        break;
                    case 2:
                        message.name = reader.string();
                        break;
                    default:
                        reader.skipType(tag & 7);
                        break;
                    }
                }
                return message;
            };
    
            /**
             * Decodes a User message from the specified reader or buffer, length delimited.
             * @function decodeDelimited
             * @memberof kop.User
             * @static
             * @param {$protobuf.Reader|Uint8Array} reader Reader or buffer to decode from
             * @returns {kop.User} User
             * @throws {Error} If the payload is not a reader or valid buffer
             * @throws {$protobuf.util.ProtocolError} If required fields are missing
             */
            User.decodeDelimited = function decodeDelimited(reader) {
                if (!(reader instanceof $Reader))
                    reader = new $Reader(reader);
                return this.decode(reader, reader.uint32());
            };
    
            /**
             * Verifies a User message.
             * @function verify
             * @memberof kop.User
             * @static
             * @param {Object.<string,*>} message Plain object to verify
             * @returns {string|null} `null` if valid, otherwise the reason why it is not
             */
            User.verify = function verify(message) {
                if (typeof message !== "object" || message === null)
                    return "object expected";
                if (message.id != null && message.hasOwnProperty("id"))
                    if (!$util.isInteger(message.id) && !(message.id && $util.isInteger(message.id.low) && $util.isInteger(message.id.high)))
                        return "id: integer|Long expected";
                if (message.name != null && message.hasOwnProperty("name"))
                    if (!$util.isString(message.name))
                        return "name: string expected";
                return null;
            };
    
            /**
             * Creates a User message from a plain object. Also converts values to their respective internal types.
             * @function fromObject
             * @memberof kop.User
             * @static
             * @param {Object.<string,*>} object Plain object
             * @returns {kop.User} User
             */
            User.fromObject = function fromObject(object) {
                if (object instanceof $root.kop.User)
                    return object;
                var message = new $root.kop.User();
                if (object.id != null)
                    if ($util.Long)
                        (message.id = $util.Long.fromValue(object.id)).unsigned = false;
                    else if (typeof object.id === "string")
                        message.id = parseInt(object.id, 10);
                    else if (typeof object.id === "number")
                        message.id = object.id;
                    else if (typeof object.id === "object")
                        message.id = new $util.LongBits(object.id.low >>> 0, object.id.high >>> 0).toNumber();
                if (object.name != null)
                    message.name = String(object.name);
                return message;
            };
    
            /**
             * Creates a plain object from a User message. Also converts values to other types if specified.
             * @function toObject
             * @memberof kop.User
             * @static
             * @param {kop.User} message User
             * @param {$protobuf.IConversionOptions} [options] Conversion options
             * @returns {Object.<string,*>} Plain object
             */
            User.toObject = function toObject(message, options) {
                if (!options)
                    options = {};
                var object = {};
                if (options.defaults) {
                    if ($util.Long) {
                        var long = new $util.Long(0, 0, false);
                        object.id = options.longs === String ? long.toString() : options.longs === Number ? long.toNumber() : long;
                    } else
                        object.id = options.longs === String ? "0" : 0;
                    object.name = "";
                }
                if (message.id != null && message.hasOwnProperty("id"))
                    if (typeof message.id === "number")
                        object.id = options.longs === String ? String(message.id) : message.id;
                    else
                        object.id = options.longs === String ? $util.Long.prototype.toString.call(message.id) : options.longs === Number ? new $util.LongBits(message.id.low >>> 0, message.id.high >>> 0).toNumber() : message.id;
                if (message.name != null && message.hasOwnProperty("name"))
                    object.name = message.name;
                return object;
            };
    
            /**
             * Converts this User to JSON.
             * @function toJSON
             * @memberof kop.User
             * @instance
             * @returns {Object.<string,*>} JSON object
             */
            User.prototype.toJSON = function toJSON() {
                return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
            };
    
            return User;
        })();
    
        kop.Login = (function() {
    
            /**
             * Properties of a Login.
             * @memberof kop
             * @interface ILogin
             * @property {string|null} [token] Login token
             */
    
            /**
             * Constructs a new Login.
             * @memberof kop
             * @classdesc Represents a Login.
             * @implements ILogin
             * @constructor
             * @param {kop.ILogin=} [properties] Properties to set
             */
            function Login(properties) {
                if (properties)
                    for (var keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                        if (properties[keys[i]] != null)
                            this[keys[i]] = properties[keys[i]];
            }
    
            /**
             * Login token.
             * @member {string} token
             * @memberof kop.Login
             * @instance
             */
            Login.prototype.token = "";
    
            /**
             * Creates a new Login instance using the specified properties.
             * @function create
             * @memberof kop.Login
             * @static
             * @param {kop.ILogin=} [properties] Properties to set
             * @returns {kop.Login} Login instance
             */
            Login.create = function create(properties) {
                return new Login(properties);
            };
    
            /**
             * Encodes the specified Login message. Does not implicitly {@link kop.Login.verify|verify} messages.
             * @function encode
             * @memberof kop.Login
             * @static
             * @param {kop.ILogin} message Login message or plain object to encode
             * @param {$protobuf.Writer} [writer] Writer to encode to
             * @returns {$protobuf.Writer} Writer
             */
            Login.encode = function encode(message, writer) {
                if (!writer)
                    writer = $Writer.create();
                if (message.token != null && Object.hasOwnProperty.call(message, "token"))
                    writer.uint32(/* id 1, wireType 2 =*/10).string(message.token);
                return writer;
            };
    
            /**
             * Encodes the specified Login message, length delimited. Does not implicitly {@link kop.Login.verify|verify} messages.
             * @function encodeDelimited
             * @memberof kop.Login
             * @static
             * @param {kop.ILogin} message Login message or plain object to encode
             * @param {$protobuf.Writer} [writer] Writer to encode to
             * @returns {$protobuf.Writer} Writer
             */
            Login.encodeDelimited = function encodeDelimited(message, writer) {
                return this.encode(message, writer).ldelim();
            };
    
            /**
             * Decodes a Login message from the specified reader or buffer.
             * @function decode
             * @memberof kop.Login
             * @static
             * @param {$protobuf.Reader|Uint8Array} reader Reader or buffer to decode from
             * @param {number} [length] Message length if known beforehand
             * @returns {kop.Login} Login
             * @throws {Error} If the payload is not a reader or valid buffer
             * @throws {$protobuf.util.ProtocolError} If required fields are missing
             */
            Login.decode = function decode(reader, length) {
                if (!(reader instanceof $Reader))
                    reader = $Reader.create(reader);
                var end = length === undefined ? reader.len : reader.pos + length, message = new $root.kop.Login();
                while (reader.pos < end) {
                    var tag = reader.uint32();
                    switch (tag >>> 3) {
                    case 1:
                        message.token = reader.string();
                        break;
                    default:
                        reader.skipType(tag & 7);
                        break;
                    }
                }
                return message;
            };
    
            /**
             * Decodes a Login message from the specified reader or buffer, length delimited.
             * @function decodeDelimited
             * @memberof kop.Login
             * @static
             * @param {$protobuf.Reader|Uint8Array} reader Reader or buffer to decode from
             * @returns {kop.Login} Login
             * @throws {Error} If the payload is not a reader or valid buffer
             * @throws {$protobuf.util.ProtocolError} If required fields are missing
             */
            Login.decodeDelimited = function decodeDelimited(reader) {
                if (!(reader instanceof $Reader))
                    reader = new $Reader(reader);
                return this.decode(reader, reader.uint32());
            };
    
            /**
             * Verifies a Login message.
             * @function verify
             * @memberof kop.Login
             * @static
             * @param {Object.<string,*>} message Plain object to verify
             * @returns {string|null} `null` if valid, otherwise the reason why it is not
             */
            Login.verify = function verify(message) {
                if (typeof message !== "object" || message === null)
                    return "object expected";
                if (message.token != null && message.hasOwnProperty("token"))
                    if (!$util.isString(message.token))
                        return "token: string expected";
                return null;
            };
    
            /**
             * Creates a Login message from a plain object. Also converts values to their respective internal types.
             * @function fromObject
             * @memberof kop.Login
             * @static
             * @param {Object.<string,*>} object Plain object
             * @returns {kop.Login} Login
             */
            Login.fromObject = function fromObject(object) {
                if (object instanceof $root.kop.Login)
                    return object;
                var message = new $root.kop.Login();
                if (object.token != null)
                    message.token = String(object.token);
                return message;
            };
    
            /**
             * Creates a plain object from a Login message. Also converts values to other types if specified.
             * @function toObject
             * @memberof kop.Login
             * @static
             * @param {kop.Login} message Login
             * @param {$protobuf.IConversionOptions} [options] Conversion options
             * @returns {Object.<string,*>} Plain object
             */
            Login.toObject = function toObject(message, options) {
                if (!options)
                    options = {};
                var object = {};
                if (options.defaults)
                    object.token = "";
                if (message.token != null && message.hasOwnProperty("token"))
                    object.token = message.token;
                return object;
            };
    
            /**
             * Converts this Login to JSON.
             * @function toJSON
             * @memberof kop.Login
             * @instance
             * @returns {Object.<string,*>} JSON object
             */
            Login.prototype.toJSON = function toJSON() {
                return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
            };
    
            return Login;
        })();
    
        kop.LoginResponse = (function() {
    
            /**
             * Properties of a LoginResponse.
             * @memberof kop
             * @interface ILoginResponse
             * @property {number|null} [code] LoginResponse code
             * @property {string|null} [msg] LoginResponse msg
             * @property {kop.IUser|null} [self] LoginResponse self
             */
    
            /**
             * Constructs a new LoginResponse.
             * @memberof kop
             * @classdesc Represents a LoginResponse.
             * @implements ILoginResponse
             * @constructor
             * @param {kop.ILoginResponse=} [properties] Properties to set
             */
            function LoginResponse(properties) {
                if (properties)
                    for (var keys = Object.keys(properties), i = 0; i < keys.length; ++i)
                        if (properties[keys[i]] != null)
                            this[keys[i]] = properties[keys[i]];
            }
    
            /**
             * LoginResponse code.
             * @member {number} code
             * @memberof kop.LoginResponse
             * @instance
             */
            LoginResponse.prototype.code = 0;
    
            /**
             * LoginResponse msg.
             * @member {string} msg
             * @memberof kop.LoginResponse
             * @instance
             */
            LoginResponse.prototype.msg = "";
    
            /**
             * LoginResponse self.
             * @member {kop.IUser|null|undefined} self
             * @memberof kop.LoginResponse
             * @instance
             */
            LoginResponse.prototype.self = null;
    
            /**
             * Creates a new LoginResponse instance using the specified properties.
             * @function create
             * @memberof kop.LoginResponse
             * @static
             * @param {kop.ILoginResponse=} [properties] Properties to set
             * @returns {kop.LoginResponse} LoginResponse instance
             */
            LoginResponse.create = function create(properties) {
                return new LoginResponse(properties);
            };
    
            /**
             * Encodes the specified LoginResponse message. Does not implicitly {@link kop.LoginResponse.verify|verify} messages.
             * @function encode
             * @memberof kop.LoginResponse
             * @static
             * @param {kop.ILoginResponse} message LoginResponse message or plain object to encode
             * @param {$protobuf.Writer} [writer] Writer to encode to
             * @returns {$protobuf.Writer} Writer
             */
            LoginResponse.encode = function encode(message, writer) {
                if (!writer)
                    writer = $Writer.create();
                if (message.code != null && Object.hasOwnProperty.call(message, "code"))
                    writer.uint32(/* id 1, wireType 0 =*/8).int32(message.code);
                if (message.msg != null && Object.hasOwnProperty.call(message, "msg"))
                    writer.uint32(/* id 2, wireType 2 =*/18).string(message.msg);
                if (message.self != null && Object.hasOwnProperty.call(message, "self"))
                    $root.kop.User.encode(message.self, writer.uint32(/* id 3, wireType 2 =*/26).fork()).ldelim();
                return writer;
            };
    
            /**
             * Encodes the specified LoginResponse message, length delimited. Does not implicitly {@link kop.LoginResponse.verify|verify} messages.
             * @function encodeDelimited
             * @memberof kop.LoginResponse
             * @static
             * @param {kop.ILoginResponse} message LoginResponse message or plain object to encode
             * @param {$protobuf.Writer} [writer] Writer to encode to
             * @returns {$protobuf.Writer} Writer
             */
            LoginResponse.encodeDelimited = function encodeDelimited(message, writer) {
                return this.encode(message, writer).ldelim();
            };
    
            /**
             * Decodes a LoginResponse message from the specified reader or buffer.
             * @function decode
             * @memberof kop.LoginResponse
             * @static
             * @param {$protobuf.Reader|Uint8Array} reader Reader or buffer to decode from
             * @param {number} [length] Message length if known beforehand
             * @returns {kop.LoginResponse} LoginResponse
             * @throws {Error} If the payload is not a reader or valid buffer
             * @throws {$protobuf.util.ProtocolError} If required fields are missing
             */
            LoginResponse.decode = function decode(reader, length) {
                if (!(reader instanceof $Reader))
                    reader = $Reader.create(reader);
                var end = length === undefined ? reader.len : reader.pos + length, message = new $root.kop.LoginResponse();
                while (reader.pos < end) {
                    var tag = reader.uint32();
                    switch (tag >>> 3) {
                    case 1:
                        message.code = reader.int32();
                        break;
                    case 2:
                        message.msg = reader.string();
                        break;
                    case 3:
                        message.self = $root.kop.User.decode(reader, reader.uint32());
                        break;
                    default:
                        reader.skipType(tag & 7);
                        break;
                    }
                }
                return message;
            };
    
            /**
             * Decodes a LoginResponse message from the specified reader or buffer, length delimited.
             * @function decodeDelimited
             * @memberof kop.LoginResponse
             * @static
             * @param {$protobuf.Reader|Uint8Array} reader Reader or buffer to decode from
             * @returns {kop.LoginResponse} LoginResponse
             * @throws {Error} If the payload is not a reader or valid buffer
             * @throws {$protobuf.util.ProtocolError} If required fields are missing
             */
            LoginResponse.decodeDelimited = function decodeDelimited(reader) {
                if (!(reader instanceof $Reader))
                    reader = new $Reader(reader);
                return this.decode(reader, reader.uint32());
            };
    
            /**
             * Verifies a LoginResponse message.
             * @function verify
             * @memberof kop.LoginResponse
             * @static
             * @param {Object.<string,*>} message Plain object to verify
             * @returns {string|null} `null` if valid, otherwise the reason why it is not
             */
            LoginResponse.verify = function verify(message) {
                if (typeof message !== "object" || message === null)
                    return "object expected";
                if (message.code != null && message.hasOwnProperty("code"))
                    if (!$util.isInteger(message.code))
                        return "code: integer expected";
                if (message.msg != null && message.hasOwnProperty("msg"))
                    if (!$util.isString(message.msg))
                        return "msg: string expected";
                if (message.self != null && message.hasOwnProperty("self")) {
                    var error = $root.kop.User.verify(message.self);
                    if (error)
                        return "self." + error;
                }
                return null;
            };
    
            /**
             * Creates a LoginResponse message from a plain object. Also converts values to their respective internal types.
             * @function fromObject
             * @memberof kop.LoginResponse
             * @static
             * @param {Object.<string,*>} object Plain object
             * @returns {kop.LoginResponse} LoginResponse
             */
            LoginResponse.fromObject = function fromObject(object) {
                if (object instanceof $root.kop.LoginResponse)
                    return object;
                var message = new $root.kop.LoginResponse();
                if (object.code != null)
                    message.code = object.code | 0;
                if (object.msg != null)
                    message.msg = String(object.msg);
                if (object.self != null) {
                    if (typeof object.self !== "object")
                        throw TypeError(".kop.LoginResponse.self: object expected");
                    message.self = $root.kop.User.fromObject(object.self);
                }
                return message;
            };
    
            /**
             * Creates a plain object from a LoginResponse message. Also converts values to other types if specified.
             * @function toObject
             * @memberof kop.LoginResponse
             * @static
             * @param {kop.LoginResponse} message LoginResponse
             * @param {$protobuf.IConversionOptions} [options] Conversion options
             * @returns {Object.<string,*>} Plain object
             */
            LoginResponse.toObject = function toObject(message, options) {
                if (!options)
                    options = {};
                var object = {};
                if (options.defaults) {
                    object.code = 0;
                    object.msg = "";
                    object.self = null;
                }
                if (message.code != null && message.hasOwnProperty("code"))
                    object.code = message.code;
                if (message.msg != null && message.hasOwnProperty("msg"))
                    object.msg = message.msg;
                if (message.self != null && message.hasOwnProperty("self"))
                    object.self = $root.kop.User.toObject(message.self, options);
                return object;
            };
    
            /**
             * Converts this LoginResponse to JSON.
             * @function toJSON
             * @memberof kop.LoginResponse
             * @instance
             * @returns {Object.<string,*>} JSON object
             */
            LoginResponse.prototype.toJSON = function toJSON() {
                return this.constructor.toObject(this, $protobuf.util.toJSONOptions);
            };
    
            return LoginResponse;
        })();
    
        return kop;
    })();

    return $root;
})(protobuf);
