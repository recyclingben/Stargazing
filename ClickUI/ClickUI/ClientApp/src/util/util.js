exports.delay = function (seconds) {
    return new Promise(resolve => setTimeout(resolve, seconds * 1000));
};

exports.randomBetween = function (num1, num2) {
    return Math.abs(num1 - num2) * Math.random() + Math.min(num1, num2);
};

exports.nil = function (val) {
    return typeof val === "undefined" || val === null || val === undefined;
};