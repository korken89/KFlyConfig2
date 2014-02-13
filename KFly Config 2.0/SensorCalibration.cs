﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFly
{
    class SensorCalibration
    {
        /*
         * Some test data.
         * Created with mx = 125, my = -250, mz = 100, 
         *              gx = 1080, gy = 1150, gz = 920 plus some noise
         */
        static public double[,] TestData = new double[1200, 3] { { 1208, -243, 105 }, { 1214, -255, 101 }, { 1194, -241, 98 }, { 1209, -252, 96 }, { 1207, -257, 93 }, { 1198, -253, 101 }, { 1203, -245, 98 }, { 1207, -245, 102 }, { 1223, -249, 93 }, { 1219, -249, 107 }, { 1198, -247, 102 }, { 1220, -257, 92 }, { 1209, -253, 105 }, { 1205, -259, 97 }, { 1209, -252, 102 }, { 1204, -245, 101 }, { 1204, -246, 96 }, { 1212, -239, 86 }, { 1212, -249, 103 }, { 1212, -261, 98 }, { 1208, -242, 97 }, { 1199, -244, 102 }, { 1209, -253, 97 }, { 1213, -249, 103 }, { 1207, -246, 101 }, { 1210, -248, 95 }, { 1209, -247, 104 }, { 1203, -251, 102 }, { 1206, -250, 106 }, { 1201, -250, 96 }, { 1209, -254, 96 }, { 1199, -243, 102 }, { 1200, -250, 103 }, { 1201, -247, 104 }, { 1190, -254, 100 }, { 1212, -255, 99 }, { 1207, -250, 103 }, { 1201, -251, 110 }, { 1212, -244, 104 }, { 1196, -247, 100 }, { 1204, -247, 97 }, { 1204, -257, 102 }, { 1207, -255, 98 }, { 1207, -249, 101 }, { 1201, -252, 104 }, { 1205, -244, 105 }, { 1204, -243, 103 }, { 1208, -253, 107 }, { 1210, -246, 109 }, { 1211, -243, 91 }, { 1201, -248, 91 }, { 1205, -242, 95 }, { 1199, -245, 90 }, { 1199, -249, 98 }, { 1205, -246, 98 }, { 1213, -249, 101 }, { 1201, -252, 101 }, { 1207, -248, 97 }, { 1204, -250, 96 }, { 1211, -252, 104 }, { 1200, -241, 98 }, { 1205, -249, 93 }, { 1208, -236, 100 }, { 1211, -251, 101 }, { 1213, -247, 98 }, { 1205, -242, 103 }, { 1198, -236, 101 }, { 1201, -248, 99 }, { 1200, -254, 100 }, { 1217, -246, 102 }, { 1202, -257, 98 }, { 1209, -251, 95 }, { 1204, -249, 99 }, { 1209, -243, 101 }, { 1201, -243, 103 }, { 1198, -250, 100 }, { 1198, -245, 98 }, { 1207, -252, 103 }, { 1204, -247, 99 }, { 1204, -240, 104 }, { 1212, -249, 100 }, { 1206, -252, 100 }, { 1206, -253, 100 }, { 1213, -246, 95 }, { 1201, -243, 95 }, { 1208, -239, 95 }, { 1209, -250, 109 }, { 1204, -249, 99 }, { 1206, -253, 105 }, { 1199, -242, 102 }, { 1199, -250, 109 }, { 1206, -252, 105 }, { 1209, -239, 94 }, { 1218, -246, 98 }, { 1202, -246, 104 }, { 1206, -255, 101 }, { 1205, -248, 111 }, { 1195, -247, 100 }, { 1203, -255, 92 }, { 1196, -251, 102 }, { 1209, -252, 95 }, { 1201, -247, 100 }, { 1206, -255, 105 }, { 1202, -245, 102 }, { 1207, -241, 101 }, { 1202, -252, 99 }, { 1207, -250, 99 }, { 1209, -247, 99 }, { 1214, -244, 86 }, { 1204, -247, 102 }, { 1194, -248, 105 }, { 1201, -252, 94 }, { 1212, -251, 92 }, { 1200, -247, 103 }, { 1210, -242, 92 }, { 1206, -247, 92 }, { 1212, -256, 104 }, { 1195, -248, 97 }, { 1204, -250, 106 }, { 1199, -251, 94 }, { 1220, -251, 97 }, { 1209, -254, 101 }, { 1212, -252, 103 }, { 1200, -254, 102 }, { 1203, -252, 101 }, { 1204, -249, 99 }, { 1210, -249, 97 }, { 1204, -251, 102 }, { 1209, -251, 98 }, { 1195, -250, 102 }, { 1203, -248, 105 }, { 1201, -244, 89 }, { 1197, -247, 109 }, { 1208, -251, 97 }, { 1206, -247, 107 }, { 1205, -251, 108 }, { 1198, -259, 93 }, { 1211, -262, 101 }, { 1207, -259, 117 }, { 1204, -251, 99 }, { 1205, -253, 102 }, { 1204, -254, 102 }, { 1196, -250, 106 }, { 1204, -253, 105 }, { 1201, -253, 95 }, { 1200, -247, 98 }, { 1199, -246, 102 }, { 1202, -238, 100 }, { 1195, -248, 113 }, { 1210, -250, 102 }, { 1208, -253, 102 }, { 1205, -251, 96 }, { 1205, -250, 93 }, { 1201, -259, 104 }, { 1210, -251, 99 }, { 1204, -246, 103 }, { 1201, -253, 96 }, { 1212, -248, 104 }, { 1204, -245, 91 }, { 1202, -245, 108 }, { 1204, -246, 104 }, { 1201, -261, 101 }, { 1199, -253, 94 }, { 1218, -245, 98 }, { 1213, -248, 101 }, { 1207, -250, 106 }, { 1199, -248, 95 }, { 1201, -244, 106 }, { 1204, -249, 114 }, { 1209, -254, 98 }, { 1198, -251, 102 }, { 1193, -251, 97 }, { 1198, -246, 91 }, { 1207, -248, 107 }, { 1207, -247, 103 }, { 1207, -249, 97 }, { 1204, -252, 101 }, { 1206, -250, 99 }, { 1203, -249, 93 }, { 1209, -257, 86 }, { 1198, -249, 100 }, { 1207, -245, 98 }, { 1201, -251, 97 }, { 1203, -242, 96 }, { 1208, -247, 91 }, { 1210, -252, 97 }, { 1199, -251, 101 }, { 1211, -251, 95 }, { 1208, -249, 102 }, { 1205, -246, 99 }, { 1204, -251, 99 }, { 1204, -248, 93 }, { 1203, -241, 96 }, { 1205, -245, 102 }, { 1205, -246, 96 }, { 1209, -254, 101 }, { 1213, -253, 110 }, { 1207, -249, 104 }, { 1204, -251, 105 }, { 1208, -258, 112 }, { -954, -258, 106 }, { -960, -245, 102 }, { -950, -254, 110 }, { -953, -240, 95 }, { -954, -261, 97 }, { -952, -248, 110 }, { -954, -250, 102 }, { -960, -254, 100 }, { -956, -248, 102 }, { -956, -254, 99 }, { -958, -246, 99 }, { -947, -250, 98 }, { -959, -258, 97 }, { -957, -262, 95 }, { -959, -251, 100 }, { -961, -244, 97 }, { -956, -249, 107 }, { -956, -250, 101 }, { -947, -247, 105 }, { -956, -257, 99 }, { -960, -243, 100 }, { -947, -254, 106 }, { -949, -250, 107 }, { -956, -252, 100 }, { -963, -249, 108 }, { -957, -260, 102 }, { -956, -242, 102 }, { -954, -247, 111 }, { -956, -240, 108 }, { -953, -243, 105 }, { -953, -250, 102 }, { -961, -255, 97 }, { -960, -259, 99 }, { -959, -250, 109 }, { -958, -249, 90 }, { -957, -245, 97 }, { -955, -255, 99 }, { -970, -246, 101 }, { -957, -250, 91 }, { -949, -254, 103 }, { -960, -246, 104 }, { -950, -249, 95 }, { -953, -251, 99 }, { -955, -241, 94 }, { -954, -247, 108 }, { -963, -251, 102 }, { -955, -249, 99 }, { -947, -250, 95 }, { -955, -249, 100 }, { -955, -248, 108 }, { -959, -253, 101 }, { -955, -254, 102 }, { -954, -248, 94 }, { -953, -241, 101 }, { -957, -252, 104 }, { -956, -255, 93 }, { -945, -251, 106 }, { -966, -252, 112 }, { -944, -252, 92 }, { -953, -248, 90 }, { -950, -251, 100 }, { -963, -253, 104 }, { -958, -252, 100 }, { -956, -257, 99 }, { -953, -254, 100 }, { -963, -257, 95 }, { -953, -253, 92 }, { -961, -255, 108 }, { -955, -254, 101 }, { -952, -252, 94 }, { -953, -256, 105 }, { -950, -250, 94 }, { -950, -248, 99 }, { -958, -255, 104 }, { -954, -244, 96 }, { -960, -258, 98 }, { -962, -252, 100 }, { -950, -257, 110 }, { -955, -252, 102 }, { -955, -257, 98 }, { -950, -253, 99 }, { -952, -254, 99 }, { -953, -256, 107 }, { -949, -260, 99 }, { -950, -247, 96 }, { -954, -240, 101 }, { -958, -239, 100 }, { -958, -252, 104 }, { -949, -250, 105 }, { -963, -252, 105 }, { -955, -250, 103 }, { -965, -255, 103 }, { -950, -260, 110 }, { -951, -250, 94 }, { -955, -256, 98 }, { -955, -260, 92 }, { -967, -238, 105 }, { -952, -251, 100 }, { -966, -248, 99 }, { -967, -244, 91 }, { -955, -258, 101 }, { -960, -248, 99 }, { -953, -247, 93 }, { -952, -250, 94 }, { -951, -249, 96 }, { -958, -253, 99 }, { -953, -246, 92 }, { -954, -254, 99 }, { -951, -254, 103 }, { -952, -246, 98 }, { -951, -242, 97 }, { -956, -252, 102 }, { -956, -253, 106 }, { -950, -248, 98 }, { -966, -245, 98 }, { -958, -245, 93 }, { -961, -249, 95 }, { -957, -253, 103 }, { -952, -247, 99 }, { -951, -250, 104 }, { -960, -243, 103 }, { -957, -257, 102 }, { -954, -249, 104 }, { -956, -244, 108 }, { -953, -252, 103 }, { -953, -253, 102 }, { -960, -255, 100 }, { -956, -251, 94 }, { -966, -251, 97 }, { -949, -249, 102 }, { -958, -254, 94 }, { -961, -250, 102 }, { -956, -253, 102 }, { -962, -252, 101 }, { -955, -254, 102 }, { -958, -251, 100 }, { -944, -232, 100 }, { -949, -233, 100 }, { -967, -244, 105 }, { -953, -246, 100 }, { -962, -256, 87 }, { -956, -253, 98 }, { -954, -253, 101 }, { -951, -247, 105 }, { -956, -248, 107 }, { -947, -249, 95 }, { -957, -252, 104 }, { -953, -255, 101 }, { -952, -250, 99 }, { -955, -244, 100 }, { -951, -248, 100 }, { -953, -249, 101 }, { -959, -247, 101 }, { -964, -247, 97 }, { -946, -258, 95 }, { -958, -250, 105 }, { -954, -252, 99 }, { -952, -260, 107 }, { -954, -257, 95 }, { -960, -252, 97 }, { -957, -246, 107 }, { -956, -251, 107 }, { -948, -249, 95 }, { -959, -254, 101 }, { -951, -261, 107 }, { -953, -250, 107 }, { -956, -255, 94 }, { -960, -244, 108 }, { -956, -241, 103 }, { -955, -252, 101 }, { -962, -244, 99 }, { -954, -245, 95 }, { -959, -257, 100 }, { -955, -250, 99 }, { -953, -249, 112 }, { -960, -247, 101 }, { -956, -247, 99 }, { -953, -247, 100 }, { -964, -247, 97 }, { -950, -252, 110 }, { -943, -248, 103 }, { -950, -261, 100 }, { -957, -256, 106 }, { -953, -244, 100 }, { -960, -253, 100 }, { -946, -248, 97 }, { -950, -242, 97 }, { -951, -254, 108 }, { -959, -255, 104 }, { -953, -252, 100 }, { -958, -248, 109 }, { -957, -251, 100 }, { -958, -255, 101 }, { -960, -258, 93 }, { -960, -252, 102 }, { -956, -251, 93 }, { -963, -253, 95 }, { -952, -251, 105 }, { -956, -255, 102 }, { -952, -247, 96 }, { 126, 889, 98 }, { 127, 901, 101 }, { 118, 903, 101 }, { 120, 896, 91 }, { 123, 900, 94 }, { 126, 901, 91 }, { 131, 897, 100 }, { 124, 900, 91 }, { 131, 902, 104 }, { 127, 895, 95 }, { 131, 904, 94 }, { 126, 905, 109 }, { 122, 902, 111 }, { 118, 900, 96 }, { 126, 894, 100 }, { 129, 902, 110 }, { 124, 894, 97 }, { 122, 908, 109 }, { 123, 903, 104 }, { 120, 901, 100 }, { 123, 898, 98 }, { 124, 889, 108 }, { 125, 894, 104 }, { 125, 902, 97 }, { 128, 912, 102 }, { 126, 904, 96 }, { 134, 902, 101 }, { 127, 897, 108 }, { 134, 901, 103 }, { 121, 905, 96 }, { 128, 899, 102 }, { 124, 909, 97 }, { 128, 894, 110 }, { 128, 898, 91 }, { 114, 900, 97 }, { 118, 890, 104 }, { 118, 903, 96 }, { 127, 900, 100 }, { 132, 900, 103 }, { 123, 904, 106 }, { 129, 905, 95 }, { 128, 904, 99 }, { 120, 898, 98 }, { 127, 900, 99 }, { 121, 902, 102 }, { 133, 894, 97 }, { 125, 895, 104 }, { 133, 898, 102 }, { 123, 900, 98 }, { 128, 909, 101 }, { 125, 904, 105 }, { 115, 898, 105 }, { 120, 897, 103 }, { 128, 908, 100 }, { 125, 914, 97 }, { 119, 901, 101 }, { 122, 898, 98 }, { 126, 893, 98 }, { 120, 905, 105 }, { 130, 901, 107 }, { 122, 906, 103 }, { 134, 895, 110 }, { 120, 901, 92 }, { 126, 897, 100 }, { 117, 906, 100 }, { 121, 897, 108 }, { 122, 900, 103 }, { 127, 898, 98 }, { 130, 905, 103 }, { 127, 902, 103 }, { 123, 895, 101 }, { 129, 897, 90 }, { 129, 895, 105 }, { 126, 903, 83 }, { 128, 902, 101 }, { 127, 892, 92 }, { 120, 890, 98 }, { 129, 900, 107 }, { 122, 903, 97 }, { 122, 901, 110 }, { 126, 892, 104 }, { 123, 892, 99 }, { 124, 904, 95 }, { 128, 899, 98 }, { 130, 898, 102 }, { 124, 907, 88 }, { 127, 896, 103 }, { 124, 901, 101 }, { 126, 902, 104 }, { 127, 904, 110 }, { 122, 896, 98 }, { 126, 902, 95 }, { 128, 914, 106 }, { 125, 892, 104 }, { 134, 902, 99 }, { 122, 899, 97 }, { 121, 896, 101 }, { 116, 896, 94 }, { 130, 902, 100 }, { 129, 912, 97 }, { 125, 896, 96 }, { 129, 903, 105 }, { 126, 897, 100 }, { 126, 910, 107 }, { 126, 903, 95 }, { 127, 899, 101 }, { 126, 903, 95 }, { 139, 900, 103 }, { 119, 906, 107 }, { 116, 899, 105 }, { 119, 894, 89 }, { 120, 901, 103 }, { 123, 908, 100 }, { 124, 894, 102 }, { 124, 899, 97 }, { 128, 905, 92 }, { 127, 890, 104 }, { 129, 903, 97 }, { 134, 904, 98 }, { 131, 901, 101 }, { 119, 896, 113 }, { 113, 900, 111 }, { 130, 900, 93 }, { 116, 905, 98 }, { 125, 903, 105 }, { 125, 898, 90 }, { 136, 908, 106 }, { 125, 900, 98 }, { 122, 896, 107 }, { 126, 898, 96 }, { 126, 904, 95 }, { 125, 901, 112 }, { 122, 904, 103 }, { 119, 900, 98 }, { 127, 912, 95 }, { 118, 892, 106 }, { 120, 898, 102 }, { 132, 900, 89 }, { 123, 900, 95 }, { 124, 907, 115 }, { 129, 901, 100 }, { 123, 899, 90 }, { 130, 900, 96 }, { 123, 904, 98 }, { 130, 897, 103 }, { 128, 902, 99 }, { 124, 892, 101 }, { 121, 900, 100 }, { 120, 895, 95 }, { 124, 897, 106 }, { 123, 901, 98 }, { 123, 894, 98 }, { 130, 895, 101 }, { 124, 892, 100 }, { 131, 889, 97 }, { 122, 899, 101 }, { 130, 906, 111 }, { 122, 899, 103 }, { 126, 899, 97 }, { 130, 895, 96 }, { 123, 908, 99 }, { 123, 899, 92 }, { 135, 906, 109 }, { 130, 902, 101 }, { 123, 899, 94 }, { 128, 905, 101 }, { 123, 905, 93 }, { 129, 899, 103 }, { 132, 906, 99 }, { 117, 909, 100 }, { 130, 900, 102 }, { 132, 899, 104 }, { 125, 902, 96 }, { 134, 902, 93 }, { 126, 904, 99 }, { 119, 899, 96 }, { 114, 904, 102 }, { 123, 905, 91 }, { 129, 901, 104 }, { 127, 901, 102 }, { 127, 894, 94 }, { 122, 894, 98 }, { 126, 893, 104 }, { 117, 894, 112 }, { 121, 899, 98 }, { 121, 892, 107 }, { 128, 911, 96 }, { 125, 904, 101 }, { 119, 890, 99 }, { 125, 894, 102 }, { 122, 899, 98 }, { 122, 891, 93 }, { 129, 898, 99 }, { 126, 905, 102 }, { 127, 899, 97 }, { 129, 900, 99 }, { 126, 895, 102 }, { 128, 897, 103 }, { 128, 893, 96 }, { 131, 902, 99 }, { 127, -1402, 104 }, { 132, -1396, 102 }, { 125, -1394, 93 }, { 125, -1395, 108 }, { 134, -1400, 97 }, { 122, -1403, 97 }, { 125, -1406, 93 }, { 130, -1399, 99 }, { 126, -1395, 99 }, { 132, -1402, 98 }, { 130, -1401, 96 }, { 126, -1397, 92 }, { 121, -1396, 115 }, { 128, -1407, 108 }, { 118, -1399, 98 }, { 126, -1399, 99 }, { 129, -1401, 102 }, { 126, -1396, 100 }, { 129, -1402, 97 }, { 135, -1404, 105 }, { 130, -1399, 94 }, { 126, -1398, 110 }, { 128, -1401, 91 }, { 127, -1396, 98 }, { 118, -1408, 104 }, { 123, -1405, 101 }, { 131, -1415, 92 }, { 125, -1397, 105 }, { 128, -1405, 101 }, { 120, -1401, 96 }, { 125, -1395, 102 }, { 128, -1400, 104 }, { 118, -1402, 111 }, { 127, -1404, 100 }, { 124, -1401, 99 }, { 120, -1397, 97 }, { 125, -1408, 90 }, { 116, -1400, 103 }, { 114, -1396, 90 }, { 119, -1407, 99 }, { 120, -1397, 99 }, { 119, -1397, 104 }, { 116, -1404, 86 }, { 126, -1408, 105 }, { 117, -1398, 97 }, { 126, -1396, 94 }, { 129, -1397, 100 }, { 125, -1396, 101 }, { 125, -1406, 98 }, { 123, -1397, 93 }, { 118, -1403, 106 }, { 125, -1403, 101 }, { 130, -1408, 96 }, { 134, -1394, 106 }, { 123, -1399, 102 }, { 117, -1395, 94 }, { 126, -1392, 103 }, { 127, -1406, 91 }, { 124, -1397, 95 }, { 119, -1400, 101 }, { 135, -1395, 102 }, { 113, -1394, 100 }, { 122, -1396, 90 }, { 118, -1404, 104 }, { 122, -1401, 106 }, { 127, -1399, 106 }, { 126, -1384, 97 }, { 121, -1394, 98 }, { 129, -1388, 95 }, { 128, -1398, 101 }, { 128, -1399, 101 }, { 123, -1397, 96 }, { 130, -1403, 105 }, { 128, -1394, 104 }, { 138, -1401, 103 }, { 130, -1407, 96 }, { 131, -1396, 93 }, { 125, -1408, 103 }, { 119, -1409, 107 }, { 123, -1399, 94 }, { 121, -1405, 89 }, { 122, -1405, 107 }, { 128, -1401, 100 }, { 122, -1402, 106 }, { 121, -1401, 105 }, { 123, -1399, 94 }, { 124, -1406, 98 }, { 127, -1406, 96 }, { 120, -1410, 101 }, { 127, -1402, 106 }, { 125, -1397, 100 }, { 132, -1404, 89 }, { 124, -1397, 106 }, { 124, -1397, 100 }, { 119, -1395, 98 }, { 118, -1395, 89 }, { 127, -1403, 102 }, { 124, -1404, 101 }, { 128, -1400, 102 }, { 121, -1403, 95 }, { 135, -1393, 96 }, { 126, -1393, 95 }, { 130, -1400, 109 }, { 124, -1402, 96 }, { 124, -1397, 99 }, { 128, -1403, 96 }, { 128, -1401, 105 }, { 119, -1403, 100 }, { 117, -1399, 100 }, { 120, -1394, 105 }, { 118, -1396, 103 }, { 125, -1394, 104 }, { 123, -1405, 89 }, { 123, -1399, 101 }, { 118, -1405, 105 }, { 129, -1398, 102 }, { 127, -1398, 105 }, { 125, -1394, 111 }, { 130, -1395, 106 }, { 121, -1397, 94 }, { 127, -1403, 96 }, { 127, -1402, 99 }, { 127, -1410, 97 }, { 121, -1402, 100 }, { 127, -1408, 106 }, { 122, -1395, 97 }, { 121, -1395, 99 }, { 119, -1399, 99 }, { 130, -1402, 103 }, { 121, -1399, 105 }, { 124, -1392, 99 }, { 119, -1393, 98 }, { 124, -1402, 105 }, { 109, -1399, 104 }, { 120, -1406, 102 }, { 118, -1398, 116 }, { 120, -1396, 100 }, { 124, -1395, 102 }, { 123, -1408, 100 }, { 135, -1397, 103 }, { 122, -1404, 97 }, { 124, -1402, 105 }, { 117, -1396, 109 }, { 123, -1399, 100 }, { 118, -1401, 100 }, { 125, -1397, 97 }, { 129, -1392, 98 }, { 127, -1403, 91 }, { 121, -1407, 100 }, { 117, -1406, 96 }, { 132, -1405, 106 }, { 135, -1403, 101 }, { 126, -1397, 100 }, { 120, -1410, 107 }, { 131, -1399, 91 }, { 122, -1404, 102 }, { 123, -1395, 95 }, { 129, -1404, 97 }, { 118, -1400, 101 }, { 115, -1405, 104 }, { 127, -1403, 96 }, { 127, -1401, 103 }, { 125, -1401, 107 }, { 127, -1403, 96 }, { 130, -1394, 95 }, { 130, -1397, 100 }, { 122, -1391, 103 }, { 129, -1401, 100 }, { 126, -1395, 96 }, { 122, -1410, 101 }, { 119, -1397, 101 }, { 128, -1398, 98 }, { 123, -1402, 94 }, { 125, -1397, 102 }, { 121, -1396, 100 }, { 117, -1395, 98 }, { 126, -1398, 97 }, { 125, -1397, 97 }, { 131, -1403, 96 }, { 129, -1405, 95 }, { 130, -1401, 106 }, { 121, -1399, 99 }, { 120, -1395, 91 }, { 119, -1404, 98 }, { 127, -1402, 105 }, { 139, -1385, 97 }, { 129, -1403, 109 }, { 121, -1390, 96 }, { 129, -1395, 96 }, { 119, -1403, 95 }, { 118, -1401, 106 }, { 129, -1401, 97 }, { 121, -1398, 102 }, { 127, -1393, 92 }, { 132, -1408, 97 }, { 127, -1390, 103 }, { 130, -1396, 94 }, { 117, -1403, 100 }, { 128, -1401, 103 }, { 136, -1399, 109 }, { 128, -249, 1023 }, { 117, -244, 1025 }, { 124, -245, 1018 }, { 123, -252, 1017 }, { 127, -251, 1020 }, { 127, -246, 1016 }, { 127, -249, 1026 }, { 123, -244, 1024 }, { 122, -253, 1025 }, { 122, -248, 1026 }, { 129, -249, 1019 }, { 116, -243, 1017 }, { 124, -257, 1023 }, { 126, -250, 1026 }, { 122, -250, 1024 }, { 127, -262, 1025 }, { 125, -244, 1028 }, { 120, -236, 1021 }, { 126, -251, 1026 }, { 124, -249, 1020 }, { 123, -252, 1024 }, { 121, -248, 1013 }, { 125, -252, 1028 }, { 116, -251, 1011 }, { 123, -242, 1021 }, { 122, -254, 1018 }, { 120, -253, 1014 }, { 128, -250, 1022 }, { 121, -251, 1022 }, { 128, -243, 1027 }, { 130, -240, 1021 }, { 124, -255, 1023 }, { 126, -253, 1021 }, { 128, -247, 1027 }, { 125, -252, 1026 }, { 128, -240, 1020 }, { 119, -242, 1021 }, { 131, -247, 1029 }, { 120, -247, 1019 }, { 116, -252, 1024 }, { 132, -246, 1016 }, { 125, -244, 1016 }, { 127, -249, 1024 }, { 123, -240, 1023 }, { 120, -247, 1025 }, { 110, -251, 1017 }, { 128, -248, 1025 }, { 124, -245, 1021 }, { 124, -252, 1020 }, { 127, -250, 1028 }, { 118, -238, 1027 }, { 121, -252, 1009 }, { 131, -245, 1022 }, { 128, -246, 1016 }, { 119, -242, 1019 }, { 114, -247, 1017 }, { 122, -262, 1028 }, { 126, -254, 1026 }, { 130, -247, 1023 }, { 125, -249, 1024 }, { 119, -240, 1022 }, { 127, -243, 1023 }, { 119, -248, 1021 }, { 120, -250, 1019 }, { 122, -258, 1023 }, { 119, -241, 1018 }, { 124, -249, 1018 }, { 121, -258, 1019 }, { 124, -248, 1020 }, { 123, -249, 1020 }, { 123, -253, 1033 }, { 122, -248, 1014 }, { 131, -254, 1015 }, { 128, -257, 1021 }, { 125, -259, 1020 }, { 118, -252, 1016 }, { 117, -253, 1022 }, { 115, -248, 1024 }, { 138, -258, 1014 }, { 130, -251, 1018 }, { 126, -258, 1014 }, { 120, -241, 1020 }, { 119, -251, 1021 }, { 128, -244, 1023 }, { 133, -248, 1015 }, { 117, -245, 1021 }, { 123, -256, 1019 }, { 125, -244, 1027 }, { 115, -243, 1022 }, { 129, -260, 1017 }, { 123, -251, 1019 }, { 135, -253, 1017 }, { 123, -249, 1027 }, { 127, -254, 1017 }, { 119, -249, 1012 }, { 122, -247, 1019 }, { 119, -249, 1026 }, { 127, -251, 1023 }, { 132, -256, 1021 }, { 122, -250, 1029 }, { 127, -253, 1017 }, { 122, -256, 1021 }, { 126, -250, 1016 }, { 131, -251, 1016 }, { 126, -249, 1018 }, { 120, -245, 1023 }, { 121, -247, 1030 }, { 124, -243, 1024 }, { 124, -254, 1015 }, { 121, -254, 1019 }, { 126, -250, 1017 }, { 131, -243, 1019 }, { 124, -243, 1026 }, { 124, -251, 1022 }, { 114, -253, 1025 }, { 121, -258, 1023 }, { 118, -247, 1016 }, { 123, -250, 1025 }, { 128, -256, 1013 }, { 133, -252, 1024 }, { 134, -259, 1023 }, { 124, -248, 1026 }, { 123, -251, 1013 }, { 129, -242, 1019 }, { 127, -257, 1020 }, { 129, -245, 1021 }, { 129, -249, 1024 }, { 118, -250, 1024 }, { 128, -246, 1023 }, { 125, -244, 1021 }, { 121, -255, 1028 }, { 122, -243, 1018 }, { 131, -256, 1021 }, { 121, -255, 1017 }, { 119, -253, 1020 }, { 126, -246, 1024 }, { 135, -255, 1014 }, { 125, -248, 1021 }, { 127, -248, 1016 }, { 120, -249, 1024 }, { 133, -247, 1010 }, { 126, -256, 1010 }, { 128, -245, 1028 }, { 120, -249, 1019 }, { 129, -246, 1010 }, { 127, -255, 1012 }, { 119, -250, 1020 }, { 125, -246, 1022 }, { 123, -245, 1022 }, { 126, -250, 1025 }, { 124, -259, 1022 }, { 124, -255, 1020 }, { 120, -249, 1025 }, { 123, -246, 1030 }, { 129, -248, 1020 }, { 134, -253, 1019 }, { 119, -239, 1025 }, { 137, -250, 1017 }, { 133, -246, 1013 }, { 126, -252, 1019 }, { 123, -256, 1021 }, { 122, -247, 1021 }, { 129, -245, 1014 }, { 122, -241, 1017 }, { 123, -253, 1017 }, { 129, -258, 1025 }, { 127, -242, 1019 }, { 129, -249, 1023 }, { 128, -256, 1023 }, { 123, -253, 1019 }, { 122, -254, 1010 }, { 129, -244, 1022 }, { 122, -247, 1024 }, { 131, -249, 1016 }, { 129, -249, 1020 }, { 126, -254, 1018 }, { 125, -245, 1029 }, { 127, -248, 1020 }, { 143, -251, 1021 }, { 124, -251, 1015 }, { 117, -251, 1016 }, { 135, -254, 1013 }, { 128, -246, 1024 }, { 122, -243, 1023 }, { 138, -238, 1016 }, { 128, -247, 1034 }, { 126, -254, 1025 }, { 121, -249, 1022 }, { 120, -255, 1030 }, { 116, -253, 1008 }, { 123, -253, 1020 }, { 120, -254, 1020 }, { 128, -248, 1021 }, { 123, -256, 1015 }, { 134, -247, 1023 }, { 133, -246, 1009 }, { 126, -245, 1021 }, { 124, -250, 1022 }, { 131, -247, 1020 }, { 119, -241, 1022 }, { 128, -250, -824 }, { 122, -251, -817 }, { 123, -241, -822 }, { 122, -263, -828 }, { 128, -252, -822 }, { 121, -238, -823 }, { 120, -252, -825 }, { 128, -253, -817 }, { 123, -256, -820 }, { 127, -246, -821 }, { 123, -261, -823 }, { 126, -254, -812 }, { 112, -247, -814 }, { 127, -250, -821 }, { 134, -247, -820 }, { 130, -249, -812 }, { 130, -247, -811 }, { 124, -256, -814 }, { 126, -249, -822 }, { 126, -258, -823 }, { 125, -246, -816 }, { 121, -252, -824 }, { 123, -247, -825 }, { 124, -253, -822 }, { 123, -248, -815 }, { 129, -245, -819 }, { 138, -260, -819 }, { 118, -259, -814 }, { 126, -259, -819 }, { 118, -246, -826 }, { 132, -248, -816 }, { 132, -254, -818 }, { 117, -243, -820 }, { 135, -249, -822 }, { 120, -258, -823 }, { 126, -255, -820 }, { 130, -257, -824 }, { 126, -245, -823 }, { 136, -253, -827 }, { 139, -256, -823 }, { 126, -253, -823 }, { 115, -254, -817 }, { 123, -246, -813 }, { 121, -248, -815 }, { 121, -248, -815 }, { 119, -248, -817 }, { 125, -250, -817 }, { 136, -255, -816 }, { 121, -247, -824 }, { 124, -254, -816 }, { 131, -255, -826 }, { 131, -255, -817 }, { 127, -246, -824 }, { 130, -245, -825 }, { 129, -259, -822 }, { 123, -251, -827 }, { 127, -249, -820 }, { 124, -253, -814 }, { 121, -246, -821 }, { 128, -249, -818 }, { 125, -251, -819 }, { 124, -253, -814 }, { 126, -247, -821 }, { 130, -246, -819 }, { 127, -248, -808 }, { 126, -251, -814 }, { 126, -245, -825 }, { 125, -255, -814 }, { 121, -249, -825 }, { 129, -243, -820 }, { 132, -251, -819 }, { 122, -254, -816 }, { 135, -240, -822 }, { 124, -251, -817 }, { 124, -248, -811 }, { 121, -254, -830 }, { 121, -249, -825 }, { 120, -248, -814 }, { 127, -250, -821 }, { 133, -248, -820 }, { 128, -244, -815 }, { 129, -257, -809 }, { 132, -249, -827 }, { 113, -260, -825 }, { 124, -250, -820 }, { 127, -240, -820 }, { 129, -245, -819 }, { 118, -250, -822 }, { 120, -252, -814 }, { 129, -247, -825 }, { 124, -245, -819 }, { 128, -250, -815 }, { 120, -242, -818 }, { 117, -254, -823 }, { 129, -260, -819 }, { 131, -254, -822 }, { 133, -251, -823 }, { 117, -252, -813 }, { 118, -254, -821 }, { 118, -243, -823 }, { 125, -258, -824 }, { 122, -252, -819 }, { 132, -255, -827 }, { 118, -252, -828 }, { 116, -253, -819 }, { 126, -248, -826 }, { 131, -255, -822 }, { 121, -254, -816 }, { 119, -249, -815 }, { 124, -245, -812 }, { 117, -259, -825 }, { 125, -241, -816 }, { 129, -254, -818 }, { 126, -247, -814 }, { 115, -245, -819 }, { 122, -243, -816 }, { 123, -253, -814 }, { 134, -250, -822 }, { 130, -251, -822 }, { 125, -253, -827 }, { 132, -249, -820 }, { 120, -248, -818 }, { 123, -252, -824 }, { 132, -249, -820 }, { 133, -237, -822 }, { 129, -250, -822 }, { 127, -246, -816 }, { 122, -252, -816 }, { 129, -253, -814 }, { 121, -252, -825 }, { 128, -247, -821 }, { 126, -254, -817 }, { 131, -259, -826 }, { 123, -250, -810 }, { 128, -252, -819 }, { 129, -249, -823 }, { 121, -250, -815 }, { 126, -246, -818 }, { 133, -245, -819 }, { 126, -257, -823 }, { 123, -258, -829 }, { 127, -247, -825 }, { 124, -248, -823 }, { 127, -255, -818 }, { 124, -244, -817 }, { 125, -250, -824 }, { 127, -249, -816 }, { 132, -249, -817 }, { 127, -246, -821 }, { 133, -242, -825 }, { 115, -251, -816 }, { 123, -259, -821 }, { 126, -245, -825 }, { 121, -255, -819 }, { 119, -244, -817 }, { 128, -253, -827 }, { 128, -243, -811 }, { 126, -254, -831 }, { 127, -256, -828 }, { 121, -245, -814 }, { 123, -257, -818 }, { 124, -247, -822 }, { 122, -248, -820 }, { 127, -251, -816 }, { 137, -252, -824 }, { 123, -240, -815 }, { 128, -252, -817 }, { 120, -251, -820 }, { 132, -255, -819 }, { 120, -254, -823 }, { 126, -253, -824 }, { 122, -241, -822 }, { 128, -252, -815 }, { 125, -253, -821 }, { 125, -252, -822 }, { 140, -246, -825 }, { 122, -257, -826 }, { 125, -247, -820 }, { 138, -257, -815 }, { 119, -251, -814 }, { 128, -259, -817 }, { 120, -247, -822 }, { 130, -247, -822 }, { 127, -253, -818 }, { 128, -248, -815 }, { 124, -249, -815 }, { 126, -244, -831 }, { 132, -249, -820 }, { 114, -245, -825 }, { 117, -251, -822 }, { 127, -253, -811 }, { 122, -250, -824 }, { 124, -251, -828 }, { 118, -246, -819 }, { 120, -250, -821 }, { 123, -254, -822 }, { 121, -251, -816 }, { 121, -244, -817 }, { 123, -252, -818 }, { 120, -253, -821 } };

        /* 
         * Uses Gaus-Newton non-linear solver to find biases and gains.
         * Input: Acceleration matrix on the form {ax, ay, az}
         * Optional: Starting guess of gain, else it will calculate the average gain and use that
         */
        public static double[,] Calibrate(double[,] acc)
        {
            return Calibrate(acc, 0);
        }

        public static double[,] Calibrate(double[,] acc, double gain)
        {
            /*
             * Formula for Gauss-Newton solver:
             * theta(k+1) = theta(k) + inv(H'*H)*H'*[y - yHAT(theta_k)]
             */

            int rows = acc.GetLength(0);

            /* Find starting guess of gain (average absolute value) */
            if (gain == 0)
            {
                for (int i = 0; i < rows; i++)
                    gain += Math.Sqrt(acc[i, 0] * acc[i, 0] + acc[i, 1] * acc[i, 1] + acc[i, 2] * acc[i, 2]);

                gain = gain / rows;
            }

            /* Starting guess of Theta: no bias and 'gain' LSB/g */
            double[,] theta = new double[6, 1] { { 0 }, { 0 }, { 0 }, { gain }, { gain }, { gain } };

            double[,] yVec = new double[rows, 1];

            for (int i = 0; i < rows; i++)
                yVec[i, 0] = 1;


            /* Fix so the abort is based on the step-size */
            for (int i = 0; i < 100; i++)
            {
                double[,] H = getH(acc, theta[0, 0], theta[1, 0], theta[2, 0], theta[3, 0], theta[4, 0], theta[5, 0]);
                double[,] yDiff = LinearAlgebra.MatrixSub(yVec, yHAT(acc, theta[0, 0], theta[1, 0], theta[2, 0], theta[3, 0], theta[4, 0], theta[5, 0]));
                double[,] hInv = LinearAlgebra.InvertMatrix(LinearAlgebra.MatrixMultiply(LinearAlgebra.MatrixTranspose(H), H));
                double[,] step = LinearAlgebra.MatrixMultiply(hInv, LinearAlgebra.MatrixTranspose(H));
                step = LinearAlgebra.MatrixMultiply(step, yDiff);

                theta = LinearAlgebra.MatrixAdd(theta, step);
            }

            return theta;
        }

        private static double[,] getH(double[,] acc, double mx, double my, double mz, double gx, double gy, double gz)
        {
            /* H = [-(2.*ax - 2.*mx)./gx.^2, -(2.*ay - 2.*my)./gy.^2, -(2.*az - 2.*mz)./gz.^2,
             * -(2.*(ax - mx).^2)./gx.^3, -(2.*(ay - my).^2)./gy.^3,   -(2.*(az - mz).^2)./gz.^3];*/
            int rows = acc.GetLength(0);
            int cols = acc.GetLength(1);

            double[,] ret = new double[rows, 6];

            for (int i = 0; i < rows; i++)
            {
                double ax = acc[i, 0];
                double ay = acc[i, 1];
                double az = acc[i, 2];

                ret[i, 0] = -2 * (ax - mx) / (gx * gx);
                ret[i, 1] = -2 * (ay - my) / (gy * gy);
                ret[i, 2] = -2 * (az - mz) / (gz * gz);

                ret[i, 3] = -2 * (ax - mx) * (ax - mx) / (gx * gx * gx);
                ret[i, 4] = -2 * (ay - my) * (ay - my) / (gy * gy * gy);
                ret[i, 5] = -2 * (az - mz) * (az - mz) / (gz * gz * gz);
            }

            return ret;
        }

        private static double[,] yHAT(double[,] acc, double mx, double my, double mz, double gx, double gy, double gz)
        {
            /* a = (ax - mx).^2./gx.^2 + (ay - my).^2./gy.^2 + (az - mz).^2./gz.^2; */
            int rows = acc.GetLength(0);

            double[,] ret = new double[rows, 1];

            for (int i = 0; i < rows; i++)
            {
                double ax = acc[i, 0];
                double ay = acc[i, 1];
                double az = acc[i, 2];

                ret[i, 0] = (ax - mx) * (ax - mx) / (gx * gx) + (ay - my) * (ay - my) / (gy * gy) + (az - mz) * (az - mz) / (gz * gz);
            }

            return ret;
        }
    }
}
