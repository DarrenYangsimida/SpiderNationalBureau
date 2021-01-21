SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for community
-- ----------------------------
DROP TABLE IF EXISTS `community`;
CREATE TABLE `community`  (
  `run_id` bigint(12) NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `community_code` bigint(12) NOT NULL COMMENT 'community code - unique',
  `community_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL COMMENT 'community name',
  `street_code` bigint(12) NOT NULL COMMENT 'street code - 父节点',
  `deleted_flag` tinyint(1) NOT NULL DEFAULT 0 COMMENT 'deleted flag - default 0',
  `created_time` datetime(0) NOT NULL COMMENT 'created time',
  `modified_time` datetime(0) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(0) COMMENT 'modified time',
  `deleted_time` datetime(0) DEFAULT NULL COMMENT 'deleted time',
  PRIMARY KEY (`run_id`) USING BTREE,
  UNIQUE INDEX `unique_index_community_code`(`community_code`) USING BTREE COMMENT '社区 code - 唯一索引'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_unicode_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
