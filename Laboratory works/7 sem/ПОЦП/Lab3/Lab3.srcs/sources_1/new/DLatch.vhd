----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/02/2023 08:17:05 PM
-- Design Name: 
-- Module Name: DLatch - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity DLatch is
    Port ( D : in STD_LOGIC;
           Q : out STD_LOGIC;
           NQ : out STD_LOGIC);
end DLatch;

architecture Struct of DLatch is
    Component RSLatch is
        Port ( R : in STD_LOGIC;
               S : in STD_LOGIC;
               Q : out STD_LOGIC;
               NQ : out STD_LOGIC);
    end component;
    
    Component Inv1 is 
        Port (
            pin : in std_logic;
            pout : out std_logic
        );
    end component;
    
    signal nd: std_logic;
begin
    inv: Inv1 port map (pin=>D, pout=>nd);
    rsl: RSLatch port map(R=>nd, S=>D, Q=>Q, NQ=>NQ);
end Struct;
